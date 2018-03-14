using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : IRoom
{
    private BaseRoom() { }
    public BaseRoom(byte floorType, byte wallType, Rect roomBounds, Building owner)
    {
        _owner = owner;
        _rect = roomBounds;
        _floorType = floorType;
        _wallType = wallType;
    }

    public Building Owner { get { return _owner; } }
    public byte FloorType { get { return _floorType; } }
    public byte WallType { get { return _wallType; } }
    public Rect Rect { get { return _rect; } }

    private readonly byte _floorType;
    private readonly byte _wallType;
    private readonly Rect _rect;
    private readonly Building _owner;

    private HashSet<Vector2> _doors;

    public byte GetTile(Vector2Int pos)
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
    public void CalculateDoors()
    {
        _doors = new HashSet<Vector2>();

        List<IRoom> adjacentRooms = GetAdjacentRooms();
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
