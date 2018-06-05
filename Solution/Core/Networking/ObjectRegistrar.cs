using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Components;
using Networking.Packages;

namespace Networking
{
    /// <summary>
    /// Contains all the network ID's for objects
    /// </summary>
    public class ObjectRegistrar
    {
        public ObjectRegistrar()
        {
            Network.EventListener.RegisterCallback((ushort)PackageIdentification.Instantiate, Instantiate);
        }

        private Dictionary<ulong, Object> _objectLookup = new Dictionary<ulong, Object>();

        public void RegisterObject(ulong id, Object obj)
        {
            _objectLookup.Add(id, obj);

            CheckForNetworkIDComponent(id, obj);
        }
        public Object GetObject(ulong id)
        {
            return _objectLookup[id];
        }
        private void CheckForNetworkIDComponent(ulong id, Object obj)
        {
            Debug.Log("Checking " + obj + " for identity");

            if(obj is GameObject gameObject)
            {
                NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();

                if(networkIdentity != null)
                {
                    networkIdentity.AssignID(id);
                }
            }
        }
        private void Instantiate(Peer peer, byte[] data)
        {
            InstantiatePackage package = data.Deserialize<InstantiatePackage>();

            Object prefab = ObjectReferenceManifest.GetObject(package.ObjectID);
            Object instantiated = Object.Instantiate(prefab);

            RegisterObject(package.NetworkID, instantiated);

            if (instantiated is GameObject)
            {
                GameObject gameObject = instantiated as GameObject;

                gameObject.transform.position = package.Position;
                gameObject.transform.rotation = package.Rotation;
            }
        }
    }
}
