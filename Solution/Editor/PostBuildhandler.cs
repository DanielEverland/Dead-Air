using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public static class PostBuildHandler
{
    private const string IGNORE_FILENAME = ".ignore";

    private static string _buildFolder;
    private static string _applicationName;

    private static Dictionary<string, string> _specialFolderNames;
    private static HashSet<string> _ignoreRelativePaths;

    [PostProcessBuild]
    private static void OnBuild(BuildTarget target, string pathToBuiltProject)
    {
        _buildFolder = Path.GetDirectoryName(pathToBuiltProject);
        _applicationName = Path.GetFileNameWithoutExtension(pathToBuiltProject);

        BuildSpecialFolderNames();

        LoadIgnorePaths();
        MoveFiles();
    }
    private static void BuildSpecialFolderNames()
    {
        _specialFolderNames = new Dictionary<string, string>();

        _specialFolderNames.Add("$DATA", $"{_applicationName}_Data");
    }
    private static void LoadIgnorePaths()
    {
        _ignoreRelativePaths = new HashSet<string>();

        string ignoreFilePath = $@"{Directories.CopyToBuildFolder}\{IGNORE_FILENAME}";

        if (File.Exists(ignoreFilePath))
        {
            string[] text = File.ReadAllLines(ignoreFilePath);

            foreach (string line in text)
            {
                if(line.StartsWith("#") || line == string.Empty)
                    continue;

                Debug.Log("IGNORE " + line);
                
                _ignoreRelativePaths.Add(line);
            }
        }
        else
        {
            Debug.LogWarning("Couldn't locate file " + ignoreFilePath);
        }
    }
    private static void MoveFiles()
    {
        Queue<MoveOrder> directoriesToCheck = new Queue<MoveOrder>();
        directoriesToCheck.Enqueue(MoveOrder.Create(Directories.CopyToBuildFolder));

        while (directoriesToCheck.Count > 0)
        {
            MoveOrder order = directoriesToCheck.Dequeue();

            foreach (string file in Directory.GetFiles(order.OriginalFolder))
            {
                string relativePath = file.Replace(Directories.CopyToBuildFolder + @"\", "");
                
                if (_ignoreRelativePaths.Contains(relativePath))
                {
                    Debug.Log("Skipping " + relativePath);
                    continue;
                }                    

                string fileName = Path.GetFileName(file);
                string targetFilePath = $@"{order.TargetFolder}\{fileName}";
                string targetDirectory = Path.GetDirectoryName(targetFilePath);

                if (fileName == IGNORE_FILENAME)
                    continue;

                Directories.EnsurePathExists(targetDirectory);

                File.Copy(file, targetFilePath, true);
            }

            foreach (string subFolder in Directory.GetDirectories(order.OriginalFolder))
            {
                directoriesToCheck.Enqueue(MoveOrder.Create(subFolder));
            }
        }
    }
    private class MoveOrder
    {
        private MoveOrder() { }

        public string TargetFolder { get; private set; }
        public string OriginalFolder { get; private set; }

        public static MoveOrder Create(string fullPath)
        {
            MoveOrder order = new MoveOrder();
            
            string relativeFolder = fullPath.Replace(Directories.CopyToBuildFolder, "");
            relativeFolder = ProcessSpecialFolderNames(relativeFolder);

            order.OriginalFolder = fullPath;
            order.TargetFolder = $@"{_buildFolder}{relativeFolder}";

            return order;
        }
        private static string ProcessSpecialFolderNames(string path)
        {
            foreach (KeyValuePair<string, string> pair in _specialFolderNames)
            {
                path = path.Replace(pair.Key, pair.Value);
            }

            return path;
        }
    }
}
