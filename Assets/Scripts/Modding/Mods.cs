using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS.Deserialization;

public static class Mods {

    public static IEnumerable<Object> Objects { get { return Deserializer.Objects.Values; } }
    public static IEnumerable<string> Keys { get { return Deserializer.Objects.Keys; } }
    public static IDictionary<string, Object> Entries { get { return Deserializer.Objects; } }

    public static void Deserialize()
    {
        Deserializer.Initialize();

        if (Application.isEditor)
        {
            DeserializeEditor();
        }
        else
        {
            DeserializeBuiltGame();
        }
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
                    Deserializer.DeserializePackage(file);
                }
            }

            foreach (string subfolder in Directory.GetDirectories(currentFolder))
            {
                directoryQueue.Enqueue(subfolder);
            }
        }
    }
    private static void DeserializeEditor()
    {
        EditorSession.Load();
    }
    public static bool Contains(string key)
    {
        if (!Deserializer.HasDeserialized)
            PollDeserializer();

        return Deserializer.KeyExists(key);
    }
    public static T GetObject<T>(string key) where T : Object
    {
        return (T)GetObject(key);
    }
    public static Object GetObject(string key)
    {
        if (!Deserializer.HasDeserialized)
            PollDeserializer();

        return Deserializer.GetObject(key);
    }
    private static void PollDeserializer()
    {
        if (Application.isPlaying)
        {
            throw new System.NullReferenceException("UMS hasn't deserialized!");
        }
        else
        {
            Deserializer.Initialize();
        }
    }
}
