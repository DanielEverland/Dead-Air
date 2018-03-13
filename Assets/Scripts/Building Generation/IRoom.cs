using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom {

	byte FloorType { get; }
    byte WallType { get; }
}
