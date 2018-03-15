using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game/Settings", order = 69)]
public class GameSettings : ScriptableObject {

    public static float IncreasedTimeScale { get { return Instance._increasedTimeScale; } }
    public static float DefaultTimeScale { get { return Instance._defaultTimeScale; } }
    public static float MinutesInDay { get { return Instance._minutesInDay; } }
    public static float InitialColonists { get { return Instance._initialColonists; } }
    public static KeyManager KeyManager { get { return Instance._keyManager; } }
    /// <summary>
    /// Measured in chunks
    /// </summary>
    public static Vector2 MapSize { get { return Instance._mapSize; } }
    /// <summary>
    /// Like MapSize, but measured in tiles
    /// </summary>
    public static Vector2 TileMap { get { return new Vector2(MapSize.x * Chunk.CHUNK_SIZE, MapSize.y * Chunk.CHUNK_SIZE); } }

    [SerializeField]
    private float _initialColonists;
    [SerializeField]
    private Vector2 _mapSize;
    [SerializeField]
    private float _buildingsPerTile;
    [SerializeField]
    private KeyManager _keyManager;
    [SerializeField]
    private float _minutesInDay;
    [SerializeField]
    private float _defaultTimeScale;
    [SerializeField]
    private float _increasedTimeScale;

    private static GameSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = StaticObjects.GetObject<GameSettings>();

            return _instance;
        }
    }
    private static GameSettings _instance;    
}
