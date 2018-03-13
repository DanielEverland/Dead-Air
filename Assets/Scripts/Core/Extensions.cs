using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

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
