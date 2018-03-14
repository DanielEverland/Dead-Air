using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    private Chunk() { }
    public Chunk(Vector2 position)
    {
        _position = position;
        _tiles = new byte[CHUNK_SIZE, CHUNK_SIZE];
        _colliders = new Dictionary<Vector2, BoxCollider>();

        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                _tiles[x, y] = (byte)TileType.Name.Grass;
            }
        }

        GameObject = GameObject.Instantiate(StaticObjects.GetObject<GameObject>("ChunkTemplate"));
    }

    public const int CHUNK_SIZE = 16;

    public byte[,] Tiles { get { return _tiles; } }
    public Vector2 Position { get { return _position; } }
    public GameObject GameObject { get; private set; }

    private Dictionary<Vector2, BoxCollider> _colliders;

    public byte GetTile(Vector2 position)
    {
        return _tiles[(int)position.x, (int)position.y];
    }
    public void SetTile(Vector2 position, byte tileID)
    {
        _tiles[(int)position.x, (int)position.y] = tileID;

        PollCollider(position, tileID);
    }
    private void PollCollider(Vector2 position, byte tileID)
    {
        TileType tile = TileType.AllTiles[tileID];

        if (tile.Impassable)
        {
            if (!_colliders.ContainsKey(position))
            {
                BoxCollider collider = GameObject.AddComponent<BoxCollider>();

                collider.center = new Vector3(position.x + 0.5f, position.y + 0.5f);

                _colliders.Add(position, collider);
            }
        }
        else
        {
            if (_colliders.ContainsKey(position))
            {
                Object.Destroy(_colliders[position]);
                _colliders.Remove(position);
            }
        }
    }

    private readonly Vector2 _position;

    private byte[,] _tiles;
}
