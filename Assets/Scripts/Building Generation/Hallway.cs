using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : IHallway {

    private Hallway() { }
	public Hallway(Building owner)
    {
        _owner = owner;
        RoomDataContainer = RoomData.All.Where(x => x.AllowHallway).Random();
    }

    public Building Owner { get { return _owner; } }
    public Rect Rect { get; set; }
    public int Thickness { get { return THICKNESS; } }
    public RoomData RoomDataContainer { get; private set; }
    public byte FloorType { get { return RoomDataContainer.FloorType; } }
    public byte WallType { get { return RoomDataContainer.WallType; } }
    public bool HasGeneratedDoors { get; private set; }
    public bool IsNested { get { return false; } }
    
    private const int THICKNESS = 3;

    private readonly int _age;
    private readonly Building _owner;

    private HashSet<Vector2> _doors;

    public void GenerateFoundation()
    {
        Utility.Loop(Rect, (x, y) =>
        {
            Vector2 pos = new Vector2(x, y);

            MapGenerator.AddTile(pos, GetTile(pos));
        });
    }
    public byte GetTile(Vector2 pos)
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
    private bool PassableAreaCheck(Vector2 pos)
    {
        int i = 0;

        Utility.Adjacent8Way(pos, x =>
        {
            if (Rect.Contains(x))
                i++;
        });

        return i >= 5;
    }
    //Hallways are root nodes, so we ignore the parent here
    public void GenerateDoors(IRoom parent)
    {
        _doors = new HashSet<Vector2>();
        
        foreach (Vector2 edgePosition in Rect.GetEdges())
        {
            if (Owner.Rect.IsEdge(edgePosition))
            {
                if (Utility.IsValidDoorPosition(Rect, edgePosition))
                {
                    _doors.Add(edgePosition);
                }
            }
        }

        HasGeneratedDoors = true;
    }
}
