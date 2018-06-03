using System.Collections.Generic;
using UMS;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Editor
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
                toReturn.Add(package.CreateFile());
            }

            return toReturn;
        }
    }
}