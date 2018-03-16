using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationSettings : ScriptableObject, ISettings {

    /// <summary>
    /// Measured in chunks
    /// </summary>
    public static Vector2 MapSize { get { return Instance._mapSize; } }
    /// <summary>
    /// Like MapSize, but measured in tiles
    /// </summary>
    public static Vector2 TileMap { get { return new Vector2(MapSize.x * Chunk.CHUNK_SIZE, MapSize.y * Chunk.CHUNK_SIZE); } }

    private static MapGenerationSettings Instance { get { return GameSettings.GetInstance<MapGenerationSettings>(); } }

    [SerializeField]
    private Vector2 _mapSize;
}
