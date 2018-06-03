using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networking.Packages;

namespace Networking
{
    /// <summary>
    /// Handles instantiate events on clients
    /// </summary>
    public static class ClientSpawner
    {
        public static void Initialize()
        {
            Client.EventListener.RegisterCallback((ushort)PackageIdentification.Instantiate, Instantiate);
        }
        private static void Instantiate(Peer peer, byte[] data)
        {
            InstantiatePackage package = data.Deserialize<InstantiatePackage>();

            Object prefab = ObjectReferenceManifest.GetObject(package.ObjectID);
            Object instantiated = Object.Instantiate(prefab);

            if(instantiated is GameObject)
            {
                GameObject gameObject = instantiated as GameObject;

                gameObject.transform.position = package.Position;
                gameObject.transform.rotation = package.Rotation;
            }
        }
    }
}
