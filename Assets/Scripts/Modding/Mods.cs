using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS.Deserialization;

public static class Mods {

    public static IEnumerable<Object> Objects { get { return Deserializer.Objects.Values; } }
    public static IEnumerable<string> Keys { get { return Deserializer.Objects.Keys; } }
    public static IDictionary<string, Object> Entries { get { return Deserializer.Objects; } }

    public static bool Contains(string key)
    {
        if (!Deserializer.HasDeserialized)
            throw new System.InvalidOperationException("UMS hasn't deserialized!");

        return Deserializer.KeyExists(key);
    }
    public static T GetObject<T>(string key) where T : Object
    {
        return (T)GetObject(key);
    }
    public static Object GetObject(string key)
    {
        if (!Deserializer.HasDeserialized)
            throw new System.InvalidOperationException("UMS hasn't deserialized!");

        return Deserializer.GetObject(key);
    }
}
