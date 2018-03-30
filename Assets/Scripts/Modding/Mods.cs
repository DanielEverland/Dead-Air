using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS;

#if UNITY_EDITOR
using UMS.Editor;
#endif

public static class Mods {

    public static IEnumerable<object> Objects { get { return ObjectContainer.Objects; } }
    public static IEnumerable<string> Keys { get { return ObjectContainer.Keys; } }

    public static void Deserialize()
    {
#if UNITY_EDITOR
        EditorSession.Load();
#else
        DeserializeBuiltGame();
#endif
    }
    private static void DeserializeBuiltGame()
    {
        Queue<string> directoryQueue = new Queue<string>();
        directoryQueue.Enqueue(Application.dataPath);

        while (directoryQueue.Count > 0)
        {
            string currentFolder = directoryQueue.Dequeue();

            foreach (string file in Directory.GetFiles(currentFolder))
            {
                if (Path.GetExtension(file) == ".mod")
                {
                    UMS.Mods.Load(file);
                }
            }

            foreach (string subfolder in Directory.GetDirectories(currentFolder))
            {
                directoryQueue.Enqueue(subfolder);
            }
        }
    }
    public static bool Contains(string key)
    {
        return ObjectContainer.ContainsKey(key);
    }
    public static T GetObject<T>(string key) where T : Object
    {
        return (T)GetObject(key);
    }
    public static object GetObject(string key)
    {
        return ObjectContainer.GetObjectFromKey(key);
    }
    private static void PollDeserializer()
    {
        if (Application.isPlaying)
        {
            throw new System.NullReferenceException("UMS hasn't deserialized!");
        }
        else
        {
            Deserialize();
        }
    }
}
