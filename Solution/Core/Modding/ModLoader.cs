using System.Collections.Generic;
using System.IO;
using UMS;
using UnityEngine;

namespace Modding
{
    public static class ModLoader
    {
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
            _cachedEditorFiles = new List<ModFile>();
            Queue<string> directoriesToCheckForMods = new Queue<string>();
            directoriesToCheckForMods.Enqueue($"{Directories.DataPath}/{Settings.ModsDirectory}");

            while (directoriesToCheckForMods.Count > 0)
            {
                string directory = directoriesToCheckForMods.Dequeue();

                foreach (string file in Directory.GetFiles(directory))
                {
                    if (Path.GetExtension(file) == UMS.Utility.MOD_EXTENSION)
                    {
                        ModFile mod = ModFile.LoadFromFile(file);

                        OutputModFile(mod);
                        _cachedEditorFiles.Add(mod);
                    }
                }

                foreach (string subFolder in Directory.GetDirectories(directory))
                {
                    directoriesToCheckForMods.Enqueue(subFolder);
                }
            }

            return _cachedEditorFiles;
        }
        private static List<ModFile> LoadInEditor()
        {
            if (_cachedEditorFiles == null)
            {
                _cachedEditorFiles = new List<ModFile>();

                foreach (ModFile file in Hooks.GetModsInProject())
                {
                    OutputModFile(file);
                    _cachedEditorFiles.Add(file);
                }
            }

            return _cachedEditorFiles;
        }
        private static void OutputModFile(ModFile file)
        {
            Output.Line("Loaded " + file);
        }
    }
}