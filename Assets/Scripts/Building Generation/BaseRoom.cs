using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : IRoom
{
    private BaseRoom() { }
    public BaseRoom(byte floorType, byte wallType, Rect roomBounds, Building owner)
    {
        _owner = owner;
        _bounds = roomBounds;
        _floorType = floorType;
        _wallType = wallType;
    }

    public Building Owner { get { return _owner; } }
    public byte FloorType { get { return _floorType; } }
    public byte WallType { get { return _wallType; } }
    public Rect Rect { get { return _bounds; } }

    private readonly byte _floorType;
    private readonly byte _wallType;
    private readonly Rect _bounds;
    private readonly Building _owner;

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
}
