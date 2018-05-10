using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour {

    /// <summary>
    /// Client's peer
    /// </summary>
    public static NetPeer Peer { get; private set; }

    private static Client _instance;

    private static EventBasedNetListener _eventListener;
    private static NetManager _netManager;

    private static bool _isInitialized;
    private const string SCENE_NAME = "Client";

    public static void Initialize()
    {
        SceneManager.LoadScene(SCENE_NAME, LoadSceneMode.Additive);

        _eventListener = new EventBasedNetListener();
        _netManager = new NetManager(_eventListener, "");

        SetupEvents();

        _isInitialized = true;
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        if (!_isInitialized)
            return;

        _netManager.PollEvents();
    }
    public static void Connect(NetEndPoint endpoint)
    {
        Debug.Log("Connecting to " + endpoint);

        _netManager.Start();

        Peer = _netManager.Connect(endpoint);
    }
    private static void SetupEvents()
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
