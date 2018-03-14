﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : IHallway {

    private Hallway() { }
	public Hallway(int age, byte floorType, byte wallType, Building owner)
    {
        _owner = owner;
        _age = age;

        FloorType = floorType;
        WallType = wallType;
    }

    public Building Owner { get { return _owner; } }
    public Rect Rect { get; set; }
    public int Thickness { get { return THICKNESS; } }
    public byte FloorType { get; set; }
    public byte WallType { get; set; }

    public int Age { get { return _age; } }

    private const int THICKNESS = 3;

    private readonly int _age;
    private readonly Building _owner;

    public byte GetTile(Vector2Int pos)
    {
        if (Owner.Rect.IsEdge(pos))
        {
            return WallType;
        }
        else
        {
            return FloorType;
        }
    }
}
