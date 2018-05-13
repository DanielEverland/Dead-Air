using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib.Utils;

/// <summary>
/// Sends data across the network using an ID
/// </summary>
public class NetworkPackage {
    
    protected NetworkPackage() { }

    /// <summary>
    /// Create empty network package
    /// </summary>
    public NetworkPackage(PackageIdentification identification)
    {
        ID = (ushort)identification;
    }

    /// <summary>
    /// Create empty network package
    /// </summary>
    public NetworkPackage(ushort id)
    {
        ID = id;
    }

    /// <summary>
    /// Create a network package given an ID and data
    /// </summary>
    public NetworkPackage(PackageIdentification identification, byte[] data)
    {
        ID = (ushort)identification;
        Data = data;
    }

    /// <summary>
    /// Create a network package given an ID and data
    /// </summary>
    public NetworkPackage(ushort id, byte[] data)
    {
        ID = id;
        Data = data;
    }

    /// <summary>
    /// Creates a network package that is serialized using Protobuf
    /// </summary>
    public NetworkPackage(PackageIdentification identification, object obj)
    {
        ID = (ushort)identification;
        Data = ByteConverter.SerializeProto(obj);
    }

    /// <summary>
    /// Creates a network package that is serialized using Protobuf
    /// </summary>
    public NetworkPackage(ushort id, object obj)
    {
        ID = id;
        Data = ByteConverter.SerializeProto(obj);
    }

    public virtual ushort ID { get; set; } = (ushort)PackageIdentification.None;

    protected virtual byte[] Data { get; set; } = new byte[0];

    public static implicit operator byte[] (NetworkPackage package)
    {
        byte[] toReturn = new byte[package.Data.Length + 2];
        System.Array.Copy(package.Data, 0, toReturn, 2, package.Data.Length);

        toReturn[0] = (byte)(package.ID >> 0);
        toReturn[1] = (byte)(package.ID >> 8);

        return toReturn;
    }
}
