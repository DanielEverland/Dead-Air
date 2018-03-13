using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static Vector2 ChunkPosToWorldPos(Vector2 chunkPos)
    {
        return new Vector2()
        {
            x = chunkPos.x * Chunk.CHUNK_SIZE,
            y = chunkPos.y * Chunk.CHUNK_SIZE,
        };
    }
	public static Vector2 WorldToChunkPos(Vector2 worldPos)
    {
        return new Vector2()
        {
            x = (int)(Mathf.Floor(worldPos.x / Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE),
            y = (int)(Mathf.Floor(worldPos.y / Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE),
        };
    }
    public static Vector2 WorldToChunkSpace(Vector2 worldPos)
    {
        return new Vector2()
        {
            x = worldPos.x % Chunk.CHUNK_SIZE,
            y = worldPos.y % Chunk.CHUNK_SIZE,
        };
    }
}
