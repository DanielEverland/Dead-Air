using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
    
    public static Rect[] Split(this Rect source, float thickness)
    {
        Rect removed;

        return source.Split(out removed, thickness);
    }
    public static Rect[] Split(this Rect source, out Rect removedRect, float thickness)
    {
        bool horizontal = UnityEngine.Random.Range(0f, 1f) > 0.5f ? true : false;
        float lerpValue = UnityEngine.Random.Range(0.3f, 0.7f);

        return source.Split(out removedRect, horizontal, thickness, lerpValue);
    }
    public static Rect[] Split(this Rect source, out Rect removedRect, bool horizontal, float thickness, float lerpValue)
    {
        if (Utility.SplitRectTooSmall(source))
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

        if (Utility.SplitRectTooSmall(firstRect) || Utility.SplitRectTooSmall(secondRect))
            return Split(source, out removedRect, horizontal, thickness, lerpValue);
        
        removedRect = new Rect()
        {
            x = horizontal ? firstArea : source.x,
            y = horizontal ? source.y : firstArea,
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
