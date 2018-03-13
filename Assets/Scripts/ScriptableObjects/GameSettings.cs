using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game/Settings", order = 69)]
public class GameSettings : ScriptableObject {

    public static float InitialColonists { get { return Instance._initialColonists; } }
    public static KeyManager KeyManager { get { return Instance._keyManager; } }
    public static Vector2 MapSize { get { return Instance._mapSize; } }

    [SerializeField]
    private float _initialColonists;
    [SerializeField]
    private Vector2 _mapSize;
    [SerializeField]
    private float _buildingsPerTile;
    [SerializeField]
    private KeyManager _keyManager;

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
