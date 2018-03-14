using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : IRoom, IHallway {

    private Hallway() { }
    public Hallway(int age)
    {
        _age = age;
    }
	public Hallway(int age, byte floorType, byte wallType)
    {
        _age = age;

        FloorType = floorType;
        WallType = wallType;
    }

    public int Thickness { get { return THICKNESS; } }
    public byte FloorType { get; set; }
    public byte WallType { get; set; }

    public int Age { get { return _age; } }

    private const int THICKNESS = 3;

    private readonly int _age;
}
