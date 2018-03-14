using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public const int SPLIT_MIN_SIZE = 6;
    
    public static void Loop(Rect rect, System.Action<int, int> action)
    {
        for (int x = (int)rect.xMin; x < (int)rect.xMax; x++)
        {
            for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
            {
                action(x, y);
            }
        }
    }
    public static TileType GetTile(byte id)
    {
        return TileType.AllTiles[id];
    }
    public static bool PollTile(Vector2 position, System.Func<TileType, bool> callback)
    {
        return callback(TileType.AllTiles[MapGenerator.GetTile(position)]);
    }
    public static void DebugRect(Rect rect)
    {
        Color color = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1), 1);

        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y), color, int.MaxValue, false);
        Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y), new Vector3(rect.x + rect.width, rect.y + rect.height), color, int.MaxValue, false);
        Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height), color, int.MaxValue, false);
        Debug.DrawLine(new Vector3(rect.x, rect.y + rect.height), new Vector3(rect.x, rect.y), color, int.MaxValue, false);
    }
    public static Bounds GetBounds(IEnumerable<Vector2> collection)
    {
        Vector2 size = new Vector2()
        {
            x = collection.Max(w => w.x) - collection.Min(w => w.x),
            y = collection.Max(w => w.y) - collection.Min(w => w.y),
        };

        Vector2 min = new Vector2()
        {
            x = collection.Min(w => w.x),
            y = collection.Min(w => w.y),
        };

        return new Bounds()
        {
            center = min + size / 2,
            size = size,
        };
    }
    public static bool SplitRectTooSmall(Rect rect, float min = SPLIT_MIN_SIZE)
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
