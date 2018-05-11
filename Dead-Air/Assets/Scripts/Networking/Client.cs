using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

public class Client {

    /// <summary>
    /// Defines whether a client has been initialized
    /// </summary>
    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// Client's peer
    /// </summary>
    public static NetPeer Peer { get; private set; }

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

    private EventBasedNetListener _eventListener;
    private NetManager _netManager;

    public static void Initialize()
    {
        if (Server.IsInitialized)
            throw new System.InvalidOperationException("Cannot create a client and a server in the same session");

        Instance.CreateClient();
        Instance.SetupEvents();
    }
    private void Update()
    {
        _netManager.PollEvents();
    }
    public void Connect(NetEndPoint endpoint)
    {
        Debug.Log("Connecting to " + endpoint);

        _netManager.Start();

        Peer = _netManager.Connect(endpoint);
    }
    private void CreateClient()
    {
        _eventListener = new EventBasedNetListener();
        _netManager = new NetManager(_eventListener, "");
    }
    private void SetupEvents()
    {
        _eventListener.PeerConnectedEvent += OnPeerConnected;
        _eventListener.PeerDisconnectedEvent += OnPeerDisconnected;
        _eventListener.NetworkErrorEvent += OnNetworkError;

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
