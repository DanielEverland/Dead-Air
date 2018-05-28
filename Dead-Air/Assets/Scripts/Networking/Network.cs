using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour {

    public static IEnumerable<Peer> Peers { get { return _peers; } }

    public static bool IsServer { get { return Server.IsInitialized; } }
    public static bool IsClient { get { return !IsServer; } }

    private static Network _instance;

    private static List<System.Action> _updateDelegates = new List<System.Action>();
    private static List<Peer> _peers = new List<Peer>();
    private static HashSet<Object> _ownedObjects = new HashSet<Object>();

    private void Awake()
    {
        _instance = this;

        Server.OnClientConnected += AddPeer;
    }
    private void Update()
    {
        for (int i = 0; i < _updateDelegates.Count; i++)
            _updateDelegates[i].Invoke();
    }
    public static new T Instantiate<T>(T obj) where T : Object
    {
        return (T)Instantiate((Object)obj);
    }
    public static bool IsOwned(Object obj)
    {
        return _ownedObjects.Contains(obj);
    }
    internal static void SetOwned(Object obj)
    {
        if (!_ownedObjects.Contains(obj))
        {
            _ownedObjects.Add(obj);
        }
    }
    public static new Object Instantiate(Object prefab)
    {
        Object instantiatedObject = null;
        
        if (IsServer)
        {
            instantiatedObject = Server.SendInstantiationPackage(prefab);
        }
        else
        {
            instantiatedObject = ClientObjectInstantiator.SendInstantiateCallToServer(prefab);
        }

        SetOwned(instantiatedObject);

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
    private static void AddPeer(Peer peer)
    {
        if (!_peers.Contains(peer))
        {
            _peers.Add(peer);
        }
    }
}
