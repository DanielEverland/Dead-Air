using ProtoBuf;
using Serialization;
using UnityEngine;

namespace Networking.Packages
{
    /// <summary>
    /// Commands an object to be instantiated across the network
    /// </summary>
    [ProtoContract]
    public class InstantiatePackage : NetworkPackage
    {
        public InstantiatePackage(Object obj)
        {
            _objectID = ObjectReferenceManifest.GetNetworkID(obj);

            Data = ByteConverter.Serialize(this);
        }

        public override ushort ID { get { return (ushort)PackageIdentification.Instantiate; } }

        public ushort ObjectID { get { return _objectID; } }

        /// <summary>
        /// The network ID of the object we wish to instantiate
        /// </summary>
        [ProtoMember(1)]
        private ushort _objectID;
    }
}