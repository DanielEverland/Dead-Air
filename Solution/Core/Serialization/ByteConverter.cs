using UMS;

namespace Serialization
{
    public static class ByteConverter
    {
        public static byte[] Serialize(object obj)
        {
            return Serializer.Serialize(obj);
        }
        public static T Deserialize<T>(byte[] array)
        {
            return Serializer.Deserialize<T>(array);
        }
    }
}