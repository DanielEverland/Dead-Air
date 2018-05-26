using System.IO;
using ProtoBuf;

public static class ByteConverter {

    public static byte[] Serialize(object obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            Serializer.Serialize(stream, obj);

            return stream.ToArray();
        }
    }
    public static T Deserialize<T>(byte[] array)
    {
        using (MemoryStream stream = new MemoryStream(array))
        {
            return Serializer.Deserialize<T>(stream);
        }
    }
}
