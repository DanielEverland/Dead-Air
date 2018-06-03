using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Assertions;
using UMS;

public static class Mod_Loading
{
    [UnityTest]
    public static IEnumerator Editor_Loading()
    {
        Assert.IsTrue(Hooks.GetModsInProject().Count > 0, "No mods were loaded");

        return null;
    }
    [UnityTest]
    public static IEnumerator Play_Mode_Loading()
    {
        List<ModFile> files = Hooks.GetModsInProject();

        Assert.IsTrue(files.Count > 0, "No mods were loaded");

        string tempPath = Path.GetTempPath() + System.Guid.NewGuid();

        Directories.EnsurePathExists(tempPath);
        
        foreach (ModFile mod in files)
        {
            mod.Save(tempPath);
        }

        int deserialized = 0;
        foreach (string savedFile in Directory.GetFiles(tempPath))
        {   
            ModFile file = ModFile.Load(savedFile);

            if(file != null)
                deserialized++;
        }

        Assert.IsTrue(deserialized == files.Count, $"We only deserialized {deserialized}/{files.Count} mod files");

        return null;
    }
}
