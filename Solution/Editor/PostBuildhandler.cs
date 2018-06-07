using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public static class PostBuildHandler
{
    private static string _buildFolder;
    private static string _applicationName;

    private static Dictionary<string, string> _specialFolderNames;

    [PostProcessBuild]
    private static void OnBuild(BuildTarget target, string pathToBuiltProject)
    {
        _buildFolder = Path.GetDirectoryName(pathToBuiltProject);
        _applicationName = Path.GetFileNameWithoutExtension(pathToBuiltProject);

        BuildSpecialFolderNames();

        MoveFiles();
    }
    private static void BuildSpecialFolderNames()
    {
        _specialFolderNames = new Dictionary<string, string>();

        _specialFolderNames.Add("$DATA", $"{_applicationName}_Data");
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
                Directory.CreateDirectory(order.TargetFolder);

                string fileName = Path.GetFileName(file);
                File.Copy(file, $@"{order.TargetFolder}\{fileName}");
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
