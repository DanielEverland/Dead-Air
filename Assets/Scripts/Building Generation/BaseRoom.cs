using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : IRoom
{
    private BaseRoom() { }
    public BaseRoom(Rect roomBounds, Building owner)
    {
        _owner = owner;
        _rect = roomBounds;

        RoomDataContainer = RoomData.All.Random();

        IsNested = true;
    }

    public Building Owner { get { return _owner; } }
    public RoomData RoomDataContainer { get; private set; }
    public byte FloorType { get { return RoomDataContainer.FloorType; } }
    public byte WallType { get { return RoomDataContainer.WallType; } }
    public Rect Rect { get { return _rect; } }
    public bool HasGeneratedDoors { get; private set; }
    public bool IsNested { get; private set; }

    private const int MAX_EXTRA_DOORS = 0;

    private readonly byte _floorType;
    private readonly byte _wallType;
    private readonly Rect _rect;
    private readonly Building _owner;

    private HashSet<Vector2> _doors;

    public void GenerateFoundation()
    {
        Utility.Loop(Rect, (x, y) =>
        {
            Vector2Int pos = new Vector2Int(x, y);
            
            MapGenerator.AddTile(pos, GetTile(pos));
        });
    }
    private byte GetTile(Vector2Int pos)
    {
        if (PotentialWall(pos.x, pos.y))
        {
            if (IsWall(pos.x, pos.y))
            {
                return WallType;
            }
            else
            {
                return FloorType;
            }
        }
        else
        {
            return FloorType;
        }
    }
    private bool PotentialWall(int x, int y)
    {
        return Rect.IsEdge(new Vector2(x, y));
    }
    private bool IsWall(int x, int y)
    {
        Vector2 basePosition = new Vector2(x, y);
        
        for (int w = -1; w <= 1; w++)
        {
            for (int m = -1; m <= 1; m++)
            {
                if ((w == 0 && m == 0) || (w != 0 && m != 0))
                    continue;

                Vector2 direction = new Vector2(w, m);

                if(!Rect.Contains(basePosition + direction))
                {
                    if (Utility.PollTile(basePosition + direction, tile => !tile.Impassable || tile.Natural))
                        return true;
                }
            }
        }

        return false;
    }
    public void GenerateDoors(IRoom parent)
    {
        _doors = new HashSet<Vector2>();

        List<IRoom> adjacentRooms = GetAdjacentRooms();

        CreateDoor(parent);

        if (adjacentRooms.Any(x => x is IHallway))
        {
            int extraDoors = Random.Range(0, MAX_EXTRA_DOORS + 1);

            for (int i = 0; i < extraDoors; i++)
            {
                CreateDoor(adjacentRooms.Random());
            }
        }
        
        HasGeneratedDoors = true;
    }
    private void CreateDoor(IRoom toRoom)
    {
        if (toRoom is IHallway)
            IsNested = false;

        List<Vector2> possiblePositions = new List<Vector2>();

        foreach (Vector2 edge in Utility.GetEdges(Rect))
        {
            if (!_doors.Contains(edge))
            {
                if(IsValidDoorPosition(edge))
                    possiblePositions.Add(edge);
            }
        }
        
        CreateDoor(possiblePositions.Random());
    }
    private bool IsValidDoorPosition(Vector2 pos)
    {
        Vector2 direction = Vector2.zero;

        Utility.Adjacent4Way(pos, x =>
        {
            if (!Rect.Contains(x))
                direction = x - pos;
        });

        Vector2[] toCheck = new Vector2[2]
        {
            pos + direction,
            pos - direction,
        };

        for (int i = 0; i < toCheck.Length; i++)
        {
            Vector2 currentPosition = toCheck[i];

            if (Utility.PollTile(toCheck[i], x => x.Impassable || x.Natural))
                return false;
        }

        return true;
    }
    private void CreateDoor(Vector2 pos)
    {
        MapGenerator.AddTile(pos, FloorType);
        _doors.Add(pos);
    }
    private List<IRoom> GetAdjacentRooms()
    {
        List<IRoom> _rooms = new List<IRoom>();

        foreach (Vector2 edge in Rect.GetEdges())
        {
            Utility.Adjacent4Way(edge, pos =>
            {
                if (!Rect.Contains(pos))
                {
                    IRoom room = Owner.Rooms.FirstOrDefault(x => x.Rect.Contains(pos));

                    if(room != default(IRoom))
                    {
                        if (!_rooms.Contains(room))
                            _rooms.Add(room);
                    }
                }
            });
        }

        return _rooms;
    }
}
