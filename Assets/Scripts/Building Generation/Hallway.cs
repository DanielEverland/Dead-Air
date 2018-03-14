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
            if (PassableAreaCheck(pos))
            {
                return FloorType;
            }
            else
            {
                return WallType;
            }            
        }
        else
        {
            return FloorType;
        }
    }
    private bool PassableAreaCheck(Vector2Int pos)
    {
        int i = 0;

        Utility.Adjacent8Way(pos, x =>
        {
            if (Rect.Contains(x + Vector2.one / 2))
                i++;
        });
        
        return i >= 5;
    }
}
