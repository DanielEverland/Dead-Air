using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates the rough layout of a building
/// </summary>
public class BuildingBlueprint {

	private BuildingBlueprint() { }
    public BuildingBlueprint(Building building)
    {
        _owner = building;   
    }

    public Building Owner { get { return _owner; } }
    public List<IRoom> Rooms { get { return _rooms; } }

    private float HALLWAY_AREA_RATE = 0.2f;
    
    private float _fullArea;
    private Building _owner;

    private HashSet<IRoom> _haveGeneratedDoors;
    private List<Rect> _roomBlocks;
    private List<IRoom> _rooms;
    private float _hallwayArea;

    public void Initialize(Rect rect)
    {
        _fullArea = rect.width * rect.height;

        CreateBlocks(rect);
        CreateRooms();
        CreateFundamentals();
        CreateDoors();
    }
    private void CreateFundamentals()
    {
        foreach (IRoom room in Rooms)
        {
            room.GenerateFoundation();
        }
    }
    private void CreateDoors()
    {
        _haveGeneratedDoors = new HashSet<IRoom>();

        List<IHallway> hallways = new List<IHallway>(_rooms.Where(x => x is IHallway).Select(x => x as IHallway));

        foreach (IHallway hallway in hallways)
        {
            CheckRooms(hallway, null);
        }
    }
    private void CheckRooms(IRoom room, IRoom parent)
    {
        if (_haveGeneratedDoors.Contains(room))
            return;

        _haveGeneratedDoors.Add(room);

        room.GenerateDoors(parent);

        foreach (IRoom adjacentRoom in GetAdjacentRooms(room))
        {
            CheckRooms(adjacentRoom, room);
        }
    }
    private IEnumerable<IRoom> GetAdjacentRooms(IRoom room)
    {
        LinkedList<IRoom> toReturn = new LinkedList<IRoom>();

        foreach (Vector2 edge in Utility.GetEdges(room.Rect))
        {
            Utility.Adjacent4Way(edge, pos =>
            {
                IRoom adjacentRoom = GetRoom(pos);

                if(adjacentRoom != null)
                {
                    if (!toReturn.Contains(adjacentRoom))
                    {
                        toReturn.AddLast(adjacentRoom);
                    }
                }
            });
        }
        
        return toReturn;
    }
    private IRoom GetRoom(Vector2 pos)
    {
        IRoom room = _rooms.FirstOrDefault(x => x.Rect.Contains(pos));

        if (room == default(IRoom))
            return null;

        return room;
    }
    private void CreateBlocks(Rect initialRect)
    {
        _rooms = new List<IRoom>();
        _roomBlocks = new List<Rect>();
        _roomBlocks.Add(initialRect);

        while (_hallwayArea / _fullArea < HALLWAY_AREA_RATE)
        {
            Split();
        }
    }
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

        Hallway hallway = new Hallway(Owner);

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
        BaseRoom room = new BaseRoom(rect, Owner);

        _rooms.Add(room);
    }
}
