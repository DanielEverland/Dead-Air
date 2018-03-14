using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

	public Building()
    {
        _size = new Vector2Int()
        {
            x = 40,
            y = 20,
        };

        _blueprint = new BuildingBlueprint(_size.x, _size.y);
    }

    public const int MIN_SIZE = 50;
    public const int MAX_SIZE = 200;

    public Vector2 Size { get { return _size; } }
    public List<IRoom> Rooms { get { return _blueprint.Rooms; } }
    
    private readonly Vector2Int _size;
    private readonly BuildingBlueprint _blueprint;

    public void Render()
    {
        foreach (IRoom room in Rooms)
        {
            if(!(room is IHallway))
            {
                Utility.Loop(room.Rect, (x, y) =>
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    byte tile = room.GetTile(pos);

                    MapGenerator.AddTile(pos, tile);
                });
            }
        }
    }
}
