using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib.Utils;

/// <summary>
/// Sends data across the network using an ID
/// </summary>
public abstract class NetworkPackage {

    public abstract ushort ID { get; }

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
