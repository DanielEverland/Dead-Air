using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

	public Building(int width, int height)
    {
        _size = new Vector2(width, height);

        Rect = new Rect(-(_size / 2), _size).Round(1);

        _blueprint = new BuildingBlueprint(this);
        _blueprint.Initialize(Rect);
    }
    
    public Rect Rect { get; private set; }
    public Vector2 Size { get { return _size; } }
    public List<IRoom> Rooms { get { return _blueprint.Rooms; } }
    
    private readonly Vector2 _size;
    private readonly BuildingBlueprint _blueprint;
}
