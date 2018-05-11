using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class OpenDirectoryMenu {

    private const int PRIORITY = 10000;

    [MenuItem("Window/Open Editor Data", priority = PRIORITY)]
    private static void OpenEditorData()
    {
        Process.Start(Directories.EditorDataPath);
    }
    [MenuItem("Window/Open Build Data", priority = PRIORITY)]
    private static void OpenBuildData()
    {
        string fullPathOfExe = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);
        string appDirectory = Path.GetDirectoryName(fullPathOfExe);
        string dataPath = $"{appDirectory}/{Path.GetFileNameWithoutExtension(fullPathOfExe)}_Data";
        
        Process.Start(dataPath);
    }
}
