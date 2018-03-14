using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    private const int SPLIT_MIN_SIZE = 6;
    
    public static bool SplitRectTooSmall(Rect rect)
    {
        return rect.width < SPLIT_MIN_SIZE || rect.height < SPLIT_MIN_SIZE;
    }
    private static float Modulo(float a, float b)
    {
        return (a % b + b) % b;
    }
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
            x = Mathf.Floor(worldPos.x / (float)Chunk.CHUNK_SIZE),
            y = Mathf.Floor(worldPos.y / (float)Chunk.CHUNK_SIZE),
        };
    }
    public static Vector2 WorldToChunkSpace(Vector2 worldPos)
    {
        return new Vector2()
        {
            x = Modulo(worldPos.x, Chunk.CHUNK_SIZE),
            y = Modulo(worldPos.y, Chunk.CHUNK_SIZE),
        };
    }
}
