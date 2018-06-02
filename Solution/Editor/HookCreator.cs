using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UMS;
using DAClient;

namespace DAEditor
{
    public static class HookCreator
    {
        [DidReloadScripts]
        private static void CreateHooks()
        {
            Hooks.GetModsInProject = GetModFilesInProject;
        }

        private static List<ModFile> GetModFilesInProject()
        {
            List<ModFile> toReturn = new List<ModFile>();

            foreach (string guid in AssetDatabase.FindAssets("t:ModPackage"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                ModPackage package = AssetDatabase.LoadAssetAtPath<ModPackage>(path);
                ModFile file = package.CreateFile();
            }

            return toReturn;
        }
    }
}
