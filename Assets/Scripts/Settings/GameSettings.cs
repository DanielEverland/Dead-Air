using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game/Settings", order = 69)]
public class GameSettings : ScriptableObject {
    
    private static GameSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Mods.GetObject<GameSettings>("GameSettings");

                if (_instance.Settings == null)
                    _instance.CreateSettingsDictionary();
            }                

            return _instance;
        }
    }
    private static GameSettings _instance;

    public static T GetInstance<T>() where T : ScriptableObject
    {
        if (Instance.Contains(typeof(T)))
        {
            return Instance.GetObject<T>();
        }
        else
        {
            return null;
        }
    }

    [SerializeField]
    private List<ObjectEntry> _entries;
    
    public bool Contains(System.Type type)
    {
        return _entries.Any(x => x.Type == type);
    }
    public T GetObject<T>() where T : ScriptableObject
    {
        if (!Contains(typeof(T)))
            return null;

        return (T)_entries.First(x => x.Type == typeof(T)).Object;
    }

    public IDictionary<System.Type, ScriptableObject> Settings
    {
        get
        {
            Dictionary<System.Type, ScriptableObject> settings = new Dictionary<System.Type, ScriptableObject>();

            foreach (ObjectEntry entry in _entries)
            {
                settings.Add(entry.Type, entry.Object);
            }

            return settings;
        }
    }
    private void CreateSettingsDictionary()
    {
        _entries = new List<ObjectEntry>();
    }

#if UNITY_EDITOR
    public void PollTypes(IEnumerable<System.Type> types)
    {
        CheckForExtraAssets(types);

        foreach (System.Type type in types)
        {
            if (!Contains(type))
                AddType(type);
        }
               
    }
    private void CheckForExtraAssets(IEnumerable<System.Type> types)
    {
        HashSet<System.Type> allowedTypes = new HashSet<System.Type>(types);
        HashSet<System.Type> loadedTypes = new HashSet<System.Type>();

        Object[] existingObjects = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));

        foreach (Object obj in existingObjects)
        {
            if (obj == this)
                continue;
            
            System.Type type = obj.GetType();
            
            if(!allowedTypes.Contains(type))
            {
                DestroyAsset(obj);
            }
            else if(loadedTypes.Contains(type))
            {
                DestroyAsset(obj);
            }
            else
            {
                if (!Contains(type))
                {
                    _entries.Add(new ObjectEntry((ScriptableObject)obj));
                }

                loadedTypes.Add(type);
            }
        }
    }
    private void AddType(System.Type type)
    {
        ScriptableObject obj = CreateInstance(type);
        obj.name = type.Name;

        AssetDatabase.AddObjectToAsset(obj, Instance);

        _entries.Add(new ObjectEntry(obj));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
        AssetDatabase.SaveAssets();
    }
    private void DestroyAsset(Object obj)
    {
        DestroyImmediate(obj, true);

        AssetDatabase.SaveAssets();
    }
#endif

    [System.Serializable]
    private struct ObjectEntry
    {
        public ObjectEntry(ScriptableObject obj)
        {
            _object = obj;
        }

        public System.Type Type { get { return Object.GetType(); } }
        public ScriptableObject Object { get { return _object; } set { _object = value; } }

        [SerializeField]
        private ScriptableObject _object;
    }
}
