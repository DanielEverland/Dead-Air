using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

/// <summary>
/// Package sent from the client to the server
/// Commands an object to be instantiated across the network
/// </summary>
[ProtoContract]
public class ClientInstantiatePackage : NetworkPackage {

    public ClientInstantiatePackage(Object obj)
    {
        RequestID = Utility.RandomShort();
        ObjectNetworkID = ObjectReferenceManifest.GetNetworkID(obj);

        Data = ByteConverter.Serialize(this);
    }
    
    public override ushort ID { get { return (ushort)PackageIdentification.ClientInstantiate; } }

    /// <summary>
    /// The request ID we use to associate respones
    /// with the object we've instantiated locally
    /// </summary>
    [ProtoMember(1)]
    public short RequestID;

    /// <summary>
    /// The network ID of the object we wish to
    /// instantiate
    /// </summary>
    [ProtoMember(2)]
    public ushort ObjectNetworkID;
}
