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
    private ScriptableObject[] _objects;
        
    public void Test()
    {
        Debug.Log("ENTRIES: " + _objects.Length);
        foreach (ScriptableObject obj in _objects)
        {
            Debug.Log(obj);
        }
    }
    public bool Contains(System.Type type)
    {
        if (type == null)
            return false;

        if (_objects == null)
        {
            Debug.LogWarning("Objects is null!");
            return false;
        }            

        return _objects.Where(x => x != null).Any(x => x.GetType() == type);
    }
    public T GetObject<T>() where T : ScriptableObject
    {
        if (!Contains(typeof(T)))
            return null;

        return (T)_objects.First(x => x.GetType() == typeof(T));
    }

    public IDictionary<System.Type, ScriptableObject> Settings
    {
        get
        {
            Dictionary<System.Type, ScriptableObject> settings = new Dictionary<System.Type, ScriptableObject>();

            foreach (ScriptableObject obj in _objects)
            {
                settings.Add(obj.GetType(), obj);
            }

            return settings;
        }
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
                    //_entries.Add(new ObjectEntry((ScriptableObject)obj));
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

        //_entries.Add(new ObjectEntry(obj));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
        AssetDatabase.SaveAssets();
    }
    private void DestroyAsset(Object obj)
    {
        DestroyImmediate(obj, true);

        AssetDatabase.SaveAssets();
    }
#endif
}
