using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS;

public static class ModLoader {

    public static List<ModFile> GetAllModFiles()
    {
        List<ModFile> toReturn = new List<ModFile>();

        if (!Application.isEditor)
        {
            LoadInApplication(toReturn);
        }
        else
        {
            LoadInEditor(toReturn);
        }

        return toReturn;
    }
    private static void LoadInApplication(List<ModFile> list)
    {
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

                    list.Add(mod);
                }
            }

            foreach (string subFolder in Directory.GetDirectories(directory))
            {
                directoriesToCheckForMods.Enqueue(subFolder);
            }
        }
    }
    private static void LoadInEditor(List<ModFile> list)
    {
        foreach (string guid in UnityEditor.AssetDatabase.FindAssets("t:ModPackage"))
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            ModPackage package = UnityEditor.AssetDatabase.LoadAssetAtPath<ModPackage>(path);

            list.Add(package.CreateFile());
        }
    }
}
