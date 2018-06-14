using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public static class BuildsTab
{
    private const string ROOT = "Builds";

    private const string Win64 = ROOT + "/Windows 64-bit";
    private const string Win64Build = Win64 + "/Build #_F1";
    private const string Win64Server = Win64 + "/Run Server #_F5";
    private const string Win64Client = Win64 + "/Run Client #_F8";

    private const string ServerStarter = "StartServer.bat";

    [MenuItem(itemName: Win64Client)]
    private static void StartWin64Client()
    {
        StartClient(BuildTarget.StandaloneWindows64);
    }
    [MenuItem(itemName: Win64Server)]
    private static void StartWin64Server()
    {
        StartServer(BuildTarget.StandaloneWindows64);
    }
    [MenuItem(itemName: Win64Build)]
    private static void BuildWin64()
    {
        Build(BuildTarget.StandaloneWindows64);
    }
    private static void StartClient(BuildTarget target)
    {
        string directory = Path.GetDirectoryName(GetPath(target));
        
        Process process = Utility.CreateNewBatch(directory);
        process.StandardInput.WriteLine(@"""Dead Air.exe"" -client");
        Utility.CloseInput(process);
    }
    private static void StartServer(BuildTarget target)
    {
        string directory = Path.GetDirectoryName(GetPath(target));
        string batchFile = $@"{directory}\{ServerStarter}";

        Utility.RunBatchFile(batchFile);
    }
    private static void Build(BuildTarget target)
    {
        Build(GetOptions(target));
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
