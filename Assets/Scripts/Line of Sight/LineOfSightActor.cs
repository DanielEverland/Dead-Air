using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightActor : MonoBehaviour {

    public float Radius { get { return _radius; } }

    [SerializeField]
    private int _radius = 5;

    private void Start()
    {
        LineOfSightManager.AddActor(this);
    }
    private void OnDestroy()
    {
        LineOfSightManager.RemoveActor(this);
    }
    public void Poll(HashSet<Vector2Int> active, HashSet<Vector2Int> passive)
    {
        Vector2Int anchor = new Vector2Int()
        {
            x = Mathf.FloorToInt(transform.position.x),
            y = Mathf.FloorToInt(transform.position.y),
        };

        for (int x = -_radius; x < _radius; x++)
        {
            for (int y = -_radius; y < _radius; y++)
            {
                Vector2Int current = new Vector2Int(anchor.x + x, anchor.y + y);

                if(Vector2Int.Distance(anchor, current) <= _radius)
                {
                    Add(active, current);
                    Add(passive, current);
                }                    
            }
        }
    }
    private void Add<T>(HashSet<T> collection, T value)
    {
        if (!collection.Contains(value))
            collection.Add(value);
    }
}
