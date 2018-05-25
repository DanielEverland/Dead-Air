using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS;

public static class ModLoader {

    private static List<ModFile> _cachedEditorFiles;

    public static List<ModFile> GetAllModFiles()
    {
        if (!Application.isEditor)
        {
            return LoadInApplication();
        }
        else
        {
            return LoadInEditor();
        }
    }
    private static List<ModFile> LoadInApplication()
    {
        List<ModFile> toReturn = new List<ModFile>();
        Queue<string> directoriesToCheckForMods = new Queue<string>();
        directoriesToCheckForMods.Enqueue($"{Directories.DataPath}/{Settings.ModsDirectory}");

        while (directoriesToCheckForMods.Count > 0)
        {
            string directory = directoriesToCheckForMods.Dequeue();
            
            foreach (string file in Directory.GetFiles(directory))
            {
                if(Path.GetExtension(file) == UMS.Utility.MOD_EXTENSION)
                {
                    ModFile mod = ModFile.Load(file);

                    Output.Line("Loaded " + mod);

                    toReturn.Add(mod);
                }
            }

            foreach (string subFolder in Directory.GetDirectories(directory))
            {
                directoriesToCheckForMods.Enqueue(subFolder);
            }
        }

        return toReturn;
    }
    private static List<ModFile> LoadInEditor()
    {
#if UNITY_EDITOR
        if (_cachedEditorFiles == null)
        {
            _cachedEditorFiles = new List<ModFile>();

            foreach (string guid in UnityEditor.AssetDatabase.FindAssets("t:ModPackage"))
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

                ModPackage package = UnityEditor.AssetDatabase.LoadAssetAtPath<ModPackage>(path);

                Output.Line("Loading " + package.FileName);

                _cachedEditorFiles.Add(package.CreateFile());
            }
        }        
#endif

        return _cachedEditorFiles;
    }
}
