using Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    private const byte MAX_OUTPUT = byte.MaxValue;

    public static Rect GetRectInScreenSpace(this RectTransform rectTransform)
    {
        Vector3[] buffer = new Vector3[4];
        rectTransform.GetWorldCorners(buffer);

        return new Rect(
                 buffer[0].x,
                 buffer[0].y,
                 buffer[2].x - buffer[0].x,
                 buffer[2].y - buffer[0].y);
    }
    public static T Deserialize<T>(this byte[] data)
    {
        return ByteConverter.Deserialize<T>(data);
    }
    public static void Output<T>(this IEnumerable<T> enumerable)
    {
        UnityEngine.Debug.Log($"Outputting {enumerable.ToString()} with {enumerable.Count()} elements");

        byte i = 0;
        foreach (object obj in enumerable)
        {
            UnityEngine.Debug.Log(obj);

            i++;
            if (i >= MAX_OUTPUT)
            {
                UnityEngine.Debug.Log("Exiting prematurely. Too many items");
            }
        }
    }
    public static void Output(this IEnumerable enumerable)
    {
        UnityEngine.Debug.Log($"Outputting {enumerable.ToString()}");

        byte i = 0;
        foreach (object obj in enumerable)
        {
            UnityEngine.Debug.Log(obj);

            i++;
            if (i >= MAX_OUTPUT)
            {
                UnityEngine.Debug.Log("Exiting prematurely. Too many items");
            }
        }
    }
    public static byte[] ToByteArray(this Guid[] guids)
    {
        byte[] data = new byte[guids.Length * 16];

        for (int i = 0; i < guids.Length; i++)
        {
            int index = i * 16;
            byte[] serializedGUID = guids[i].ToByteArray();

            for (int j = 0; j < serializedGUID.Length; j++)
            {
                data[index + j] = serializedGUID[j];
            }
        }

        return data;
    }
}