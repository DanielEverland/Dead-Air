using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates the rough layout of a building
/// </summary>
public class BuildingBlueprint {

	private BuildingBlueprint() { }
    public BuildingBlueprint(float width, float height)
    {
        _roomBlocks = new List<Rect>();
        _roomBlocks.Add(new Rect(-(width / 2), -(height / 2), width, height));

        _fullArea = width * height;

        while (_hallwayArea / _fullArea < HALLWAY_AREA_RATE)
        {
            Split();
        }
        
        for (int x = (int)-(width / 2); x < width / 2; x++)
        {
            for (int y = (int)-(height / 2); y < height / 2; y++)
            {
                Vector2 position = new Vector2(x, y);

                if (_roomBlocks.Any(w => w.Contains(position)))
                {
                    Vector2 chunkPos = Utility.WorldToChunkPos(position);
                    Vector2 localPos = Utility.WorldToChunkSpace(position);

                    Chunk chunk = MapGenerator.GetChunk(chunkPos);

                    chunk.SetTile(localPos, 2);
                }
            }
        }
    }

    public List<Rect> RoomBlocks { get { return _roomBlocks; } }

    private float HALLWAY_AREA_RATE = 0.2f;

    private readonly int _width;
    private readonly int _height;
    private readonly float _fullArea;

    private List<Rect> _roomBlocks;
    private int _hallwayAge;
    private float _hallwayArea;

    public void Split()
    {
        List<Rect> toSplit = new List<Rect>(_roomBlocks);
        _roomBlocks.Clear();

        foreach (Rect rect in toSplit)
        {
            Split(rect);
        }
    }
    public void Split(Rect rect)
    {
        if (Utility.SplitRectTooSmall(rect))
        {
            _roomBlocks.Add(rect);
            return;
        }
            

        Hallway hallway = new Hallway(_hallwayAge);
        _hallwayAge++;

        Rect removedRect;
        Rect[] newRectangles = rect.Split(out removedRect, hallway.Thickness);

        if(newRectangles == null)
        {
            _roomBlocks.Add(rect);
            return;
        }

        _hallwayArea += removedRect.width * removedRect.height;

        for (int i = 0; i < newRectangles.Length; i++)
        {
            _roomBlocks.Add(newRectangles[i]);
        }
    }
}
