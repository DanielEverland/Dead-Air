using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directories {

    private const string FOLDER_NAME_LOCAL_GAME = "Dead Air";
    private const string FOLDER_NAME_LOCAL_COMPANY = "Everland Games";
    private const string FOLDER_NAME_PROFILES = "Profiles";
    private const string FOLDER_NAME_SERVER = "Server";
    private const string FOLDER_NAME_EDITOR_DATA = "Editor Data";

    public static string Persistant
    {
        get
        {
            return $@"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)}\{FOLDER_NAME_LOCAL_COMPANY}\{FOLDER_NAME_LOCAL_GAME}";
        }
    }
    public static string Profiles
    {
        get
        {
            return $@"{Server}\{FOLDER_NAME_PROFILES}";
        }
    }
    public static string Server
    {
        get
        {
            return $@"{DataPath}\{FOLDER_NAME_SERVER}";
        }
    }
    public static string DataPath
    {
        get
        {
            if (Application.isEditor)
            {
                return EditorDataPath;
            }
            else
            {
                return Application.dataPath;
            }
        }
    }
    public static string EditorDataPath
    {
        get
        {
            return $@"{Directory.GetParent(Application.dataPath).FullName}\{FOLDER_NAME_EDITOR_DATA}";
        }
    }

    public static void EnsurePathExists(string path)
    {
        Directory.CreateDirectory(path);
    }
}
