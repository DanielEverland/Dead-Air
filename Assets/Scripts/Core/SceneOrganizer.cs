using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneOrganizer {
    
    private static GameObject Root
    {
        get
        {
            if (_root == null)
                CreateRoot();

            return _root;
        }
    }
    private static GameObject _root;

    private static Dictionary<string, GameObject> _groups = new Dictionary<string, GameObject>();

    public static void Add(string groupName, GameObject obj)
    {
        if (!_groups.ContainsKey(groupName))
            CreateGroup(groupName);

        obj.transform.SetParent(_groups[groupName].transform);
    }
    private static void CreateGroup(string groupName)
    {
        if (_groups.ContainsKey(groupName))
            return;

        GameObject obj = new GameObject(groupName);
        obj.transform.SetParent(Root.transform);

        _groups.Add(groupName, obj);
    }
    private static void CreateRoot()
    {
        _root = new GameObject("Organizer");        
    }
}
