using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building
{
    public Building()
    {
        _tiles = new Dictionary<Vector2, TileType.Names>();
    }
    
    public bool StaticPosition { get; protected set; }
    public Vector2 Position { get; set; }
    public Rect Rect { get; private set; }

    private Dictionary<Vector2, TileType.Names> _tiles;

    public virtual void Place()
    {
        foreach (KeyValuePair<Vector2, TileType.Names> keyValuePair in _tiles)
        {
            Vector2 worldPosition = keyValuePair.Key + Position;

            Vector2 chunkPos = Utility.WorldToChunkPos(worldPosition);
            Vector2 localPos = Utility.WorldToChunkSpace(worldPosition);
            
            Chunk chunk = MapGenerator.GetChunk(chunkPos);

            chunk.SetTile(localPos, keyValuePair.Value);
        }
    }
    protected void Add(Vector2 pos, TileType.Names name)
    {
        _tiles.Add(pos, name);

        CheckRect(pos);
    }
    private void CheckRect(Vector2 pos)
    {
        Rect rectangle = Rect;

        if(pos.x < rectangle.x)
        {
            rectangle.x = pos.x;
        }
        if(pos.y < rectangle.y)
        {
            rectangle.y = pos.y;
        }

        if(pos.x > rectangle.x + rectangle.width)
        {
            rectangle.width = pos.x - rectangle.x;
        }
        if (pos.y > rectangle.y + rectangle.height)
        {
            rectangle.height = pos.y - rectangle.y;
        }
    }
}
