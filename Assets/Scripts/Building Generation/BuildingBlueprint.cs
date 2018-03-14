using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates the rough layout of a building
/// </summary>
public class BuildingBlueprint {

	private BuildingBlueprint() { }
    public BuildingBlueprint(Rect rect, Building building)
    {
        _owner = building;
        _rooms = new List<IRoom>();
        _roomBlocks = new List<Rect>();
        _roomBlocks.Add(rect);

        _fullArea = rect.width * rect.height;

        while (_hallwayArea / _fullArea < HALLWAY_AREA_RATE)
        {
            Split();
        }

        CreateRooms();
    }

    public Building Owner { get { return _owner; } }
    public List<IRoom> Rooms { get { return _rooms; } }

    private float HALLWAY_AREA_RATE = 0.2f;
    
    private readonly float _fullArea;
    private readonly Building _owner;
    
    private List<Rect> _roomBlocks;
    private List<IRoom> _rooms;
    private int _hallwayAge;
    private float _hallwayArea;

    public void Split()
    {
        List<Rect> toSplit = new List<Rect>(_roomBlocks);
        _roomBlocks.Clear();

        foreach (Rect rect in toSplit)
        {
            Split(rect);
        }
    }
    public void Split(Rect rect)
    {
        if (Utility.SplitRectTooSmall(rect))
        {
            _roomBlocks.Add(rect);
            return;
        }

        Hallway hallway = new Hallway(_hallwayAge, (byte)TileType.Name.WoodFloor, (byte)TileType.Name.WoodWall, Owner);
        _hallwayAge++;

        Rect removedRect;
        Rect[] newRectangles = rect.Split(out removedRect, hallway.Thickness);
        
        if(newRectangles == null)
        {
            _roomBlocks.Add(rect);
            return;
        }

        hallway.Rect = removedRect.Round(1);
        _rooms.Add(hallway);

        _hallwayArea += removedRect.width * removedRect.height;

        for (int i = 0; i < newRectangles.Length; i++)
        {
            _roomBlocks.Add(newRectangles[i].Round(1));
        }
    }
    private void CreateRooms()
    {
        foreach (Rect block in _roomBlocks)
        {
            RoomChunk chunk = new RoomChunk(block);

            foreach (Rect roomSegment in chunk.Segments)
            {
                AddRoom(roomSegment);
            }
        }
    }
    private void AddRoom(Rect rect)
    {
        BaseRoom room = new BaseRoom((byte)TileType.Name.WoodFloor, (byte)TileType.Name.WoodWall, rect, Owner);

        _rooms.Add(room);
    }
}
