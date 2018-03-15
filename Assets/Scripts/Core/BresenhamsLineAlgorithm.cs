using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BresenhamsLineAlgorithm {

    public static bool Raycast(Vector2 start, Vector2 end, System.Func<Vector2, bool> callback)
    {
        return Raycast(start, end, false, callback);
    }
    public static bool Raycast(Vector2 start, Vector2 end, bool getLast, System.Func<Vector2, bool> callback)
    {
        foreach (Vector2 pos in Get(start, end))
        {
            if((pos != start && pos != end) || !getLast)
            {
                if (callback(pos))
                    return true;
            }            
        }

        return false;
    }
    public static IEnumerable<Vector2Int> Get(Vector2 start, Vector2 end)
    {
        return Get(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y), Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));
    }
    public static IEnumerable<Vector2Int> Get(int x, int y, int x2, int y2)
    {
        LinkedList<Vector2Int> toReturn = new LinkedList<Vector2Int>();

        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            toReturn.AddLast(new Vector2Int(x, y));

            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }

        return toReturn;
    }
}
