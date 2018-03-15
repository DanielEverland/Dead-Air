using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator {
    
    private static Dictionary<Vector2, Chunk> _chunks;
        
    public static void Initialize()
    {
        _chunks = new Dictionary<Vector2, Chunk>();

        CreateChunks();
        Building building = new Building(40, 25);

        RenderChunks();
    }
    public static byte GetTile(Vector2 worldPos)
    {
        Vector2 chunkPos = Utility.WorldToChunkPos(worldPos);
        Vector3 localPos = Utility.WorldToChunkSpace(worldPos);

        Chunk chunk = _chunks[chunkPos];
        return chunk.GetTile(localPos);
    }
    public static void AddTile(Vector2 worldPos, byte tileType)
    {
        if (tileType == 0)
            throw new System.ArgumentException("Invalid ID: " + tileType);

        Vector2 chunkPos = Utility.WorldToChunkPos(worldPos);
        Vector3 localPos = Utility.WorldToChunkSpace(worldPos);
        
        Chunk chunk = _chunks[chunkPos];
        chunk.SetTile(localPos, tileType);
    }
    private static void RenderChunks()
    {
        foreach (Chunk chunk in _chunks.Values)
        {
            ChunkCreator.RenderChunk(chunk);
        }
    }
    public static Chunk GetChunk(Vector2 chunkPos)
    {
        if (_chunks.ContainsKey(chunkPos))
        {
            return _chunks[chunkPos];
        }
        else
        {
            return null;
        }
    }
    private static void CreateChunks()
    {
        int xStart = -Mathf.FloorToInt(GameSettings.MapSize.x / 2);
        int xEnd = Mathf.CeilToInt(GameSettings.MapSize.x / 2);

        int yStart = -Mathf.FloorToInt(GameSettings.MapSize.y / 2);
        int yEnd = Mathf.CeilToInt(GameSettings.MapSize.y / 2);

        for (int x = xStart; x < xEnd; x++)
        {
            for (int y = yStart; y < yEnd; y++)
            {
                CreateChunk(new Vector2(x, y));
            }
        }
    }
    private static Chunk CreateChunk(Vector2 chunkPosition)
    {
        Chunk chunk = new Chunk(chunkPosition);

        _chunks.Add(chunkPosition, chunk);

        return chunk;
    }
}
