using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shadows {

    private const string ROOF_KEY = "RoofShadowCaster";
    
    private static Dictionary<Vector2, GameObject> _roofs = new Dictionary<Vector2, GameObject>();
 
    public static GameObject Add(Vector2 position)
    {
        GameObject prefab = StaticObjects.GetObject<GameObject>(ROOF_KEY);
        GameObject obj = GameObject.Instantiate(prefab);

        obj.transform.position = position;

        _roofs.Add(position, obj);

        return obj;
    }
    public static void Remove(Vector2 pos)
    {
        GameObject.Destroy(_roofs[pos]);
        _roofs.Remove(pos);
    }
    public static bool Contains(Vector2 pos)
    {
        return _roofs.ContainsKey(pos);
    }
    public static void Add(Rect rect)
    {
        Utility.Loop(rect, (x, y) =>
        {
            Add(new Vector2(x, y));
        });
    }
}
