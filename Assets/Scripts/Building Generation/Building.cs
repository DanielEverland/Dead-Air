using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

	public Building()
    {
        _rooms = new Dictionary<Vector2, IRoom>();
        _size = new Vector2Int()
        {
            x = 40,
            y = 20,
        };

        BuildingBlueprint blueprint = new BuildingBlueprint(_size.x, _size.y);
    }

    public const int MIN_SIZE = 50;
    public const int MAX_SIZE = 200;

    public Vector2 Size { get { return _size; } }

    private Dictionary<Vector2, IRoom> _rooms;
    private readonly Vector2Int _size;

    public void Add(Vector2 position, IRoom room)
    {
        _rooms.Set(position, room);
    }
}
