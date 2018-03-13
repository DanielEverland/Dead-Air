using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    private Chunk() { }
    public Chunk(Vector2 position)
    {
        _position = position;
        _tiles = new TileType.Names[CHUNK_SIZE, CHUNK_SIZE];

        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                _tiles[x, y] = TileType.Names.Grass;
            }
        }
    }

    public const int CHUNK_SIZE = 16;

    public TileType.Names[,] Tiles { get { return _tiles; } }
    public Vector2 Position { get { return _position; } }
    public GameObject GameObject { get; set; }

    public Vector2[] GetUVs()
    {
        Vector2[] uvs = new Vector2[CHUNK_SIZE * CHUNK_SIZE * 4];

        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                int index = (y * CHUNK_SIZE + x) * 4;
                Vector2[] tileUVs = Atlas.Instance.GetUVs(_tiles[x, y]);

                for (int w = 0; w < 4; w++)
                {
                    uvs[index + w] = tileUVs[w];
                }
            }
        }

        return uvs;
    }

    private readonly Vector2 _position;

    private TileType.Names[,] _tiles;
}
