using System.Collections.Generic;
using UMS;
using UnityEditor;
using UnityEditor.Callbacks;

namespace DeadAirEditor
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

                //We have to simulate loading the files like we do in the built version
                ModFile file = package.CreateFile();
                byte[] serialized = Serializer.Serialize(file);

                toReturn.Add(ModFile.LoadFromBinary(serialized));
            }

            return toReturn;
        }
    }
}