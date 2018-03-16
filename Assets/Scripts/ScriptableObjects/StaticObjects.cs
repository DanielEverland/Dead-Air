using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS.Deserialization;

public class StaticObjects : MonoBehaviour {

    [SerializeField]
    private StaticObjectsValueContainer obj;

    private static Dictionary<string, Object> _dictionary; 

    private void Awake()
    {
        _dictionary = new Dictionary<string, Object>();

        foreach (Entry entry in obj.Entries)
        {
            Add(entry.Key, entry.Object);
        }

        Deserializer.OnHasInitialized += LoadMods;
    }
    private void LoadMods()
    {
        foreach (KeyValuePair<string, Object> entry in Deserializer.Objects)
        {
            Add(entry.Key, entry.Value);
        }
    }
    private void Add(string key, Object obj)
    {
        if (_dictionary.ContainsKey(key))
            Debug.LogError("Duplicate object key " + key);

        _dictionary.Add(key, obj);
    }
    /// <summary>
    /// Returns object with key
    /// </summary>
    public static T GetObject<T>(string key) where T : Object
    {
        if (_dictionary.ContainsKey(key))
        {
            return (T)_dictionary[key];
        }
        else
        {
            throw new System.NullReferenceException();
        }
    }

    /// <summary>
    /// Returns first instance of type
    /// </summary>
    public static T GetObject<T>() where T : Object
    {
        foreach (Object obj in _dictionary.Values)
        {
            if (obj.GetType() == typeof(T))
                return (T)obj;
        }

        throw new System.NullReferenceException();
    }

    [CreateAssetMenu(fileName = "StaticObjects.asset", menuName = "Game/StaticObjects", order = 69)]
    public class StaticObjectsValueContainer : ScriptableObject
    {
        public List<Entry> Entries;
    }
    [System.Serializable]
    public struct Entry
    {
        public string Key;
        public Object Object;
    }
}
