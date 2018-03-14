using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom {
    
    Rect Rect { get; }
	byte FloorType { get; }
    byte WallType { get; }

    byte GetTile(Vector2Int pos);
}
