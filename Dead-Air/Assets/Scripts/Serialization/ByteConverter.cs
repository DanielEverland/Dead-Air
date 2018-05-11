using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class ByteConverter {

    public static byte[] Serialize(object obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }        
    }
    public static T Deserialize<T>(byte[] array)
    {
        try
        {
            using (MemoryStream stream = new MemoryStream(array))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                return (T)formatter.Deserialize(stream);
            }
        }
        catch (System.Exception)
        {
            return default(T);
        }        
    }
}
