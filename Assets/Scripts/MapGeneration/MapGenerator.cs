using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator {

    private static Queue<Vector2> _chunkCreationQueue;
    private static Dictionary<Vector2, Chunk> _chunks;

    private const int CHUNK_RENDER_SIZE = 4;
    
    public static void Initialize()
    {
        _chunkCreationQueue = new Queue<Vector2>();
        _chunks = new Dictionary<Vector2, Chunk>();
    }
    public static void Update()
    {
        Dequeue();
    }
    public static void Poll()
    {
        Poll(Camera.main);
    }
    private static void Dequeue()
    {
        if (_chunkCreationQueue.Count > 0)
        {
            Chunk chunk = CreateChunk(_chunkCreationQueue.Dequeue());

            ChunkCreator.Create(chunk);
        }
    }
    private static Chunk CreateChunk(Vector2 chunkPosition)
    {
        Chunk chunk = new Chunk(chunkPosition);

        _chunks.Add(chunkPosition, chunk);

        return chunk;
    }
	private static void Poll(Camera camera)
    {
        Vector2 centerChunkPosition = Utility.WorldToChunkPos(camera.transform.position);

        for (int x = -CHUNK_RENDER_SIZE; x < CHUNK_RENDER_SIZE; x++)
        {
            for (int y = -CHUNK_RENDER_SIZE; y < CHUNK_RENDER_SIZE; y++)
            {
                Vector2 currentChunkPos = centerChunkPosition + new Vector2(x, y);

                if(IsMissing(currentChunkPos))
                {
                    _chunkCreationQueue.Enqueue(currentChunkPos);
                }
            }
        }
    }
    private static bool IsMissing(Vector2 chunkPos)
    {
        return !_chunkCreationQueue.Contains(chunkPos) && !_chunks.ContainsKey(chunkPos);
    }
}
