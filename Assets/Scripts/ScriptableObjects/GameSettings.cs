using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game/Settings", order = 69)]
public class GameSettings : ScriptableObject {

    public const int NEEDS_MAX_VALUE = 1000;

    public static float StartHour { get { return Instance._startHour; } }
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

    public static int StartHunger { get { return Instance._startHunger; } }
    public static int StartThirst { get { return Instance._startThirst; } }
    public static int StartRest { get { return Instance._startRest; } }
    public static int HungerDegradation { get { return Instance._hungerDegradation; } }
    public static int ThirstDegradation { get { return Instance._thirstDegradation; } }
    public static int RestDegradation { get { return Instance._restDegradation; } }

    [Header("General")]
    [SerializeField]
    private float _initialColonists;

    [Header("Map Generation")]
    [SerializeField]
    private Vector2 _mapSize;
    [SerializeField]
    private float _buildingsPerTile;

    [Header("Input")]
    [SerializeField]
    private KeyManager _keyManager;

    [Header("Time")]
    [SerializeField]
    private float _minutesInDay;
    [SerializeField]
    private float _defaultTimeScale;
    [SerializeField]
    private float _increasedTimeScale;
    [SerializeField]
    private float _startHour;

    
    [Header("Needs")]
    [Range(0, NEEDS_MAX_VALUE)]
    [SerializeField]
    private int _startHunger;
    [Range(0, NEEDS_MAX_VALUE)]
    [SerializeField]
    private int _startThirst;
    [Range(0, NEEDS_MAX_VALUE)]
    [SerializeField]
    private int _startRest;

    [Space()]
    [SerializeField]
    private int _hungerDegradation;
    [SerializeField]
    private int _thirstDegradation;
    [SerializeField]
    private int _restDegradation;
    
    private static GameSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = Mods.GetObject<GameSettings>("GameSettings");

            return _instance;
        }
    }
    private static GameSettings _instance;    
}
