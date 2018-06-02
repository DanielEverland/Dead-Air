using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class ClearEditorDataMenu {

        [MenuItem(itemName: "Assets/Clear Editor Data", priority = 100)]
        private static void ClearEditorData()
        {
            if(!Directory.Exists(Directories.EditorDataPath))
            {
                Debug.Log(Directories.EditorDataPath + " does not exist");
                return;
            }

            Debug.Log("Deleting " + Directories.EditorDataPath);

            Directory.Delete(Directories.EditorDataPath, true);
        }
    }
}
