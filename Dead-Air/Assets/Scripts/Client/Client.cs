using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using UMS;

public class Client {

    /// <summary>
    /// Defines whether a client has been initialized
    /// </summary>
    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// Client's peer
    /// </summary>
    public static NetPeer Peer { get; private set; }

    /// <summary>
    /// Does this client have a connection to a server?
    /// </summary>
    public static bool IsConnected { get { return Peer != null; } }

    /// <summary>
    /// Handles receiving of data from other peers
    /// </summary>
    public static PackageEventListener EventListener { get; private set; }

    private static Client Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Client();

            return _instance;
        }
    }
    private static Client _instance;
    
    private NetManager _netManager;

    public static void Initialize()
    {
        if (Server.IsInitialized)
            throw new System.InvalidOperationException("Cannot create a client and a server in the same session");

        Session.Initialize();
        
        Instance.CreateClient();
        Instance.SetupEvents();

        ClientInitializer.Initialize();
    }
    public static void Connect(NetEndPoint endpoint)
    {
        if (IsConnected)
            throw new System.InvalidOperationException("We already have an established connection to the server");

        Debug.Log("Connecting to " + endpoint);

        Instance._netManager.Start();

        Peer = Instance._netManager.Connect(endpoint);
    }
    private void Update()
    {
        _netManager.PollEvents();
    }    
    private void CreateClient()
    {
        EventListener = new PackageEventListener();
        _netManager = new NetManager(EventListener, "");
    }
    private void SetupEvents()
    {
        EventListener.PeerConnectedEvent += OnPeerConnected;
        EventListener.PeerDisconnectedEvent += OnPeerDisconnected;
        EventListener.NetworkErrorEvent += OnNetworkError;

        Network.RegisterUpdateHandler(Update);
    }
    private static void OnPeerConnected(NetPeer peer)
    {
        Debug.Log($"Connected to {peer.EndPoint} with ID {peer.ConnectId}");
    }
    private static void OnPeerDisconnected(NetPeer peer, DisconnectInfo info)
    {
        Debug.Log($"Disconnected from server with message {info.Reason}");
    }
    private static void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Debug.LogError($"Error ({socketErrorCode}) from {endPoint}");
    }
}
