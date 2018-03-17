using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        if (source == null) return false;

        return source.IndexOf(toCheck, comp) >= 0;
    }
    public static bool IsEdge(this Rect source, Vector2 position)
    {
        return position.x == source.x || position.y == source.y || position.x == source.xMax - 1 || position.y == source.yMax - 1;
    }
    public static int Floor(this int a, int b)
    {
        return (int)Mathf.Floor((float)a / (float)b) * b;
    }
    public static float Floor(this float a, float b)
    {
        return Mathf.Floor(a / b) * b;
    }
    public static Vector2 Round(this Vector2 vector, float value)
    {
        return new Vector2()
        {
            x = Mathf.Round(vector.x / value) * value,
            y = Mathf.Round(vector.y / value) * value,
        };
    }
    public static Rect Round(this Rect rect, float value)
    {
        return new Rect()
        {
            x = Mathf.Round(rect.x / value) * value,
            y = Mathf.Round(rect.y / value) * value,
            width = Mathf.Round(rect.width / value) * value,
            height = Mathf.Round(rect.height / value) * value,
        };
    }
    public static Rect Floor(this Rect rect, float value)
    {
        return new Rect()
        {
            x = Mathf.Floor(rect.x / value) * value,
            y = Mathf.Floor(rect.y / value) * value,
            width = Mathf.Floor(rect.width / value) * value,
            height = Mathf.Floor(rect.height / value) * value,
        };
    }
    public static Rect[] Split(this Rect source, float thickness, int minSize = Utility.SPLIT_MIN_SIZE)
    {
        Rect removed;

        return source.Split(out removed, thickness, minSize);
    }
    public static Rect[] Split(this Rect source, out Rect removedRect, float thickness, int minSize = Utility.SPLIT_MIN_SIZE)
    {
        bool horizontal = source.width > source.height ? true : false;
        float lerpValue = UnityEngine.Random.Range(0.2f, 0.8f);
        
        return source.Split(out removedRect, horizontal, thickness, lerpValue, minSize);
    }
    public static Rect[] Split(this Rect source, out Rect removedRect, bool horizontal, float thickness, float lerpValue, int minSize = Utility.SPLIT_MIN_SIZE)
    {
        if (Utility.SplitRectTooSmall(source, minSize))
            throw new ArgumentException("Rect too small!");

        float areaDistance = horizontal ? source.width : source.height;

        float firstArea = (areaDistance * lerpValue) - (thickness / 2);
        float secondArea = (areaDistance - (areaDistance * lerpValue)) - (thickness / 2);

        Rect firstRect = new Rect()
        {
            x = source.x,
            y = source.y,
            width = horizontal ? firstArea : source.width,
            height = horizontal ? source.height : firstArea,
        };

        Rect secondRect = new Rect()
        {
            x = horizontal ? source.x + firstArea + thickness : source.x,
            y = horizontal ? source.y : source.y + firstArea + thickness,
            width = horizontal ? secondArea : source.width,
            height = horizontal ? source.height : secondArea,
        };

        if (Utility.SplitRectTooSmall(firstRect, minSize) || Utility.SplitRectTooSmall(secondRect, minSize))
        {
            removedRect = Rect.zero;
            return null;
        }
        
        removedRect = new Rect()
        {
            x = horizontal ? source.x + firstArea : source.x,
            y = horizontal ? source.y : source.y + firstArea,
            width = horizontal ? thickness : source.width,
            height = horizontal ? source.height : thickness,
        };

        return new Rect[2] { firstRect, secondRect };
    }    
    public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
        else
        {
            dictionary[key] = value;
        }
    }
    public static Vector3 Floor(this Vector3 vector, int value)
    {
        return new Vector3()
        {
            x = (int)(Mathf.Floor(vector.x / value) * value),
            y = (int)(Mathf.Floor(vector.y / value) * value),
            z = (int)(Mathf.Floor(vector.z / value) * value),
        };
    }
    public static Vector2 Floor(this Vector2 vector, int value)
    {
        return new Vector2()
        {
            x = (int)(Mathf.Floor(vector.x / value) * value),
            y = (int)(Mathf.Floor(vector.y / value) * value),
        };
    }
    public static T Random<T>(this IEnumerable<T> collection)
    {
        int index = UnityEngine.Random.Range(0, collection.Count());

        return collection.ElementAt(index);
    }
    public static T[] Copy<T>(this T[] array)
    {
        T[] newArray = new T[array.Length];
        array.CopyTo(newArray, 0);

        return newArray;
    }
    public static Rect Shrink(this Rect rect, float amount)
    {
        return new Rect()
        {
            x = rect.x + amount / 2,
            y = rect.y + amount / 2,
            width = rect.width - amount / 2,
            height = rect.height - amount / 2,
        };
    }
}
