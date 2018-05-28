using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Package sent from the server to the client
/// Tells the client to instantiate an object
/// </summary>
[ProtoContract]
public class ServerInstantiatePackage : NetworkPackage {

    public ServerInstantiatePackage(ushort objectNetworkID, int instanceID, short requestID = -1)
    {
        RequestID = requestID;
        ObjectNetworkID = objectNetworkID;
        InstanceID = instanceID;

        Data = ByteConverter.Serialize(this);
    }

    public override ushort ID { get { return (ushort)PackageIdentification.ServerInstantiate; } }

    /// <summary>
    /// The request this package is associated with
    /// Will be -1 if the object is instantiated from the server
    /// </summary>
    [ProtoMember(1)]
    public short RequestID;

    /// <summary>
    /// The network ID of the object we want to instantiate
    /// </summary>
    [ProtoMember(2)]
    public ushort ObjectNetworkID;

    /// <summary>
    /// The instance ID used to link objects across clients
    /// </summary>
    [ProtoMember(3)]
    public int InstanceID;
}
