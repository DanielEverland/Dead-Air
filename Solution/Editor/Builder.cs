using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class Builder
{
    private const string ROOT = "Build";

    private const string Win64 = ROOT + "/Windows 64-bit";

    [MenuItem(itemName: Win64)]
    private static void BuildWin64()
    {
        BuildPlayerOptions options = GetOptions(BuildTarget.StandaloneWindows64);

        Build(options);
    }
    private static void Build(BuildPlayerOptions options)
    {
        string targetDirectory = Path.GetDirectoryName(options.locationPathName);

        if (Directory.Exists(targetDirectory))
        {
            Directory.Delete(targetDirectory, true);
        }

        BuildPipeline.BuildPlayer(options);
    }
    private static BuildPlayerOptions GetOptions(BuildTarget target)
    {
        return new BuildPlayerOptions()
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            locationPathName = GetPath(target),
            target = target,
        };
    }
    private static string GetPath(BuildTarget target)
    {
        return $@"{Directories.BuildFolder}\{target.ToString()}\{PlayerSettings.productName}.exe";
    }
}
