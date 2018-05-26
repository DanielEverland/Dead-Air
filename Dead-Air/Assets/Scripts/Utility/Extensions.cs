using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class Extensions {

    private const byte MAX_OUTPUT = byte.MaxValue;

    public static T Deserialize<T>(this byte[] data)
    {
        return ByteConverter.Deserialize<T>(data);
    }
    public static void Output<T>(this IEnumerable<T> enumerable)
    {
        Debug.Log($"Outputting {enumerable.ToString()} with {enumerable.Count()} elements");

        byte i = 0;
        foreach (object obj in enumerable)
        {
            Debug.Log(obj);

            i++;
            if (i >= MAX_OUTPUT)
            {
                Debug.Log("Exiting prematurely. Too many items");
            }
        }
    }
    public static void Output(this IEnumerable enumerable)
    {
        Debug.Log($"Outputting {enumerable.ToString()}");

        byte i = 0;        
        foreach (object obj in enumerable)
        {
            Debug.Log(obj);

            i++;
            if(i >= MAX_OUTPUT)
            {
                Debug.Log("Exiting prematurely. Too many items");
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
