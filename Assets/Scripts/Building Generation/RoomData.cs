using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData.asset", menuName = "Game/RoomData", order = 69)]
public class RoomData : ScriptableObject {

	public static List<RoomData> All
    {
        get
        {
            if (_all == null)
                _all = new List<RoomData>(Resources.LoadAll<RoomData>("RoomData"));

            return _all;
        }
    }
    private static List<RoomData> _all;

    public static RoomData Random()
    {
        return Random(x => true);
    }
    public static RoomData Random(System.Func<RoomData, bool> predicate)
    {
        throw new System.NotImplementedException();
    }

    public byte WallType { get { return (byte)_wallType; } }
    public byte FloorType { get { return (byte)_floorType; } }
    public bool AllowHallway { get { return _allowHallway; } }
    public int SpawnChance { get { return _spawnChance; } }

    [SerializeField]
    private TileType.Name _floorType;
    [SerializeField]
    private TileType.Name _wallType;
    [SerializeField]
    private bool _allowHallway;
    [Range(0, 100)]
    [SerializeField]
    private int _spawnChance;
}
