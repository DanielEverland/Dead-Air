using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public static class Extensions {
    
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
