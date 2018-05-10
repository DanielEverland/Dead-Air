using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib;
using UnityEngine;

public class NetworkingManager : MonoBehaviour {

    /// <summary>
    /// Client's peer
    /// </summary>
    public static NetPeer LocalPeer { get; private set; }

    private static NetworkingManager _instance;

    private EventBasedNetListener _eventListener;
    private NetManager _client;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        Server.Initialize();

        Connect(new NetEndPoint("localhost", 9050));
    }
    private void Update()
    {
        _client.PollEvents();
    }
    public void Connect(NetEndPoint endpoint)
    {
        _eventListener = new EventBasedNetListener();
        _client = new NetManager(_eventListener, "");

        SetupEvents();

        Debug.Log("Connecting to " + endpoint);

        _client.Start();
        LocalPeer = _client.Connect(endpoint);
    }
    private void SetupEvents()
    {
        _eventListener.PeerConnectedEvent += OnPeerConnected;
        _eventListener.PeerDisconnectedEvent += OnPeerDisconnected;
        _eventListener.NetworkErrorEvent += OnNetworkError;
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
