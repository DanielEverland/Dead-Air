using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class ClientObjectInstantiator {

    static ClientObjectInstantiator()
    {
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ServerInstantiate, OnInstantiateObject);
    }

    private static Dictionary<short, Object> _objectsAwaitingNetworkdID = new Dictionary<short, Object>();

	internal static Object SendInstantiateCallToServer(Object prefab)
    {
        Object instantiated = Object.Instantiate(prefab);

        ClientInstantiatePackage package = new ClientInstantiatePackage(prefab);
        _objectsAwaitingNetworkdID.Add(package.RequestID, instantiated);

        Client.Peer.SendReliableUnordered(package);

        return instantiated;
    }
    private static void OnInstantiateObject(Peer peer, byte[] data)
    {
        ServerInstantiatePackage package = data.Deserialize<ServerInstantiatePackage>();

        Object obj = null;

        if (_objectsAwaitingNetworkdID.ContainsKey(package.RequestID))
        {
            obj = _objectsAwaitingNetworkdID[package.RequestID];
            _objectsAwaitingNetworkdID.Remove(package.RequestID);
        }
        else
        {
            ushort objectId = package.ObjectNetworkID;
            obj = Object.Instantiate(ObjectReferenceManifest.GetObject(objectId));
        }

        obj.name = "Client";
        Utility.InitializeNetworkBehaviours(obj, package.InstanceID);
    }
}
