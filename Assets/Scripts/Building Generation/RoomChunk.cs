using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChunk {

	private RoomChunk() { }
    public RoomChunk(Rect rect)
    {
        _bounds = rect;

        _roomSegments = new List<Rect>()
        {
            rect,
        };
        
        while (_currentFails < MAX_FAILS)
        {
            Split();
        }
    }

    public Rect Bounds { get { return _bounds; } }
    public IEnumerable<Rect> Segments { get { return _roomSegments; } }
    
    private const int MAX_FAILS = 10;

    private readonly Rect _bounds;
    private List<Rect> _roomSegments;
    private int _currentFails;

    private void Split()
    {
        List<Rect> toSplit = new List<Rect>(_roomSegments);
        _roomSegments.Clear();

        foreach (Rect rect in toSplit)
        {
            Split(rect);
        }
    }
    private void Split(Rect rect)
    {
        if (Utility.SplitRectTooSmall(rect))
        {
            Failed(rect);
            return;
        }
        
        Rect removedRect;
        Rect[] newRectangles = rect.Split(out removedRect, 0);

        if (newRectangles == null)
        {
            Failed(rect);
            return;
        }
        
        for (int i = 0; i < newRectangles.Length; i++)
        {
            _roomSegments.Add(newRectangles[i].Round(1));
        }
    }
    private void Failed(Rect rect)
    {
        _roomSegments.Add(rect);
        _currentFails++;
    }
}
