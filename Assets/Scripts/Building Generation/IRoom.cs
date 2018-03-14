using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom {
    
    Rect Rect { get; }
    bool HasGeneratedDoors { get; }
    byte FloorType { get; }
    byte WallType { get; }

    void GenerateFoundation();
    void GenerateDoors(IRoom parent);
}
