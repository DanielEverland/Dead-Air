using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom {
    
    RoomData RoomDataContainer { get; }
    Rect Rect { get; }
    bool HasGeneratedDoors { get; }
    byte FloorType { get; }
    byte WallType { get; }
    bool IsNested { get; }

    void GenerateFoundation();
    void GenerateDoors(IRoom parent);
}
