using System.Collections.Generic;
using UnityEngine;
using Networking.Packages;
using Debugging;

namespace Networking
{
    public class Network : MonoBehaviour
    {
        /// <summary>
        /// Handles receiving of data from other peers
        /// </summary>
        public static PackageEventListener EventListener { get; private set; }
        
        public static IEnumerable<Peer> Peers { get { return _peers; } }

        public static event System.Action ApplicationQuit;

        private static Network _instance;
        
        private static List<System.Action> _updateDelegates = new List<System.Action>();
        private static HashSet<Object> _ownedObjects = new HashSet<Object>();
        private static List<Peer> _peers = new List<Peer>();
        private static ObjectRegistrar _objectRegistrar;

        private void Awake()
        {
            _instance = this;

            CoreOutput.Initialize();
            EventListener = new PackageEventListener();
            _objectRegistrar = new ObjectRegistrar();
        }
        private void Update()
        {
            for (int i = 0; i < _updateDelegates.Count; i++)
                _updateDelegates[i].Invoke();
        }        
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return (GameObject)Instantiate((Object)prefab, position, rotation);
        }
        public static GameObject Instantiate(GameObject prefab)
        {
            return (GameObject)Instantiate((Object)prefab, prefab.transform.position, prefab.transform.rotation);
        }
        public static new T Instantiate<T>(T obj) where T : Object
        {
            return (T)Instantiate(obj, Vector3.zero, Quaternion.identity);
        }
        public static bool IsOwned(Object obj)
        {
            return _ownedObjects.Contains(obj);
        }
        private static void SetOwned(Object obj)
        {
            if (!_ownedObjects.Contains(obj))
            {
                _ownedObjects.Add(obj);
            }
        }
        public static new Object Instantiate(Object prefab, Vector3 position, Quaternion rotation)
        {
            ulong networkID = Utility.RandomULong();
            Object instantiatedObject = Object.Instantiate(prefab);

            SetOwned(instantiatedObject);

            _objectRegistrar.RegisterObject(networkID, instantiatedObject);

            foreach (Peer peer in Peers)
            {
                peer.SendReliableUnordered(new InstantiatePackage(prefab, position, rotation, networkID));
            }

            return instantiatedObject;
        }
        public static void RegisterUpdateHandler(System.Action callback)
        {
            _updateDelegates.Add(callback);
        }
        public static void UnregisterUpdateHandler(System.Action callback)
        {
            _updateDelegates.Remove(callback);
        }
        private void OnApplicationQuit()
        {
            ApplicationQuit?.Invoke();
            ApplicationQuit = null;
        }
        public static void AddPeer(Peer peer)
        {
            if (!_peers.Contains(peer))
                _peers.Add(peer);
        }
        public static void RemovePeer(Peer peer)
        {
            if (_peers.Contains(peer))
                _peers.Remove(peer);
        }
    }
}