using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;

public class Server : MonoBehaviour {

    private static ServerConfiguration _configuration;
    private static NetManager _netManager;
    private static EventBasedNetListener _eventListener;

    private static Server _instance;

    private static bool _isInitialized;
    private const string SCENE_NAME = "Server";

    public static void Initialize()
    {
        SceneManager.LoadScene(SCENE_NAME, LoadSceneMode.Additive);

        CreateServer();
        SetupEvents();

        Output.Header("Successfully started server");

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
    private static void CreateServer()
    {
        _eventListener = new EventBasedNetListener();
        _netManager = new NetManager(_eventListener, ServerConfiguration.MaximumConnections, ServerConfiguration.Password);

        _netManager.UpdateTime = ServerConfiguration.UpdateInterval;
        _netManager.Start(ServerConfiguration.Port);

        Output.DebugLine($"Max Connections: {ServerConfiguration.MaximumConnections}");
        Output.DebugLine($"Port: {ServerConfiguration.Port}");
        Output.DebugLine($"Update Interval: {ServerConfiguration.UpdateInterval}");
        Output.DebugLine($"Password: {ServerConfiguration.Password}");

        Output.DebugLine();
    }
    private static void SetupEvents()
    {
        _eventListener.PeerConnectedEvent += OnPeerConnected;
        _eventListener.PeerDisconnectedEvent += OnPeerDisconnected;
        _eventListener.NetworkErrorEvent += OnNetworkError;
    }
    private static void OnPeerConnected(NetPeer peer)
    {
        Output.Line($"Connection {peer.ConnectId} received from {peer.EndPoint}");
    }
    private static void OnPeerDisconnected(NetPeer peer, DisconnectInfo info)
    {
        Output.Line($"Connection {peer.ConnectId} disconnected with message {info.Reason}");
    }
    private static void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Output.Line($"Error ({socketErrorCode}) from {endPoint}");
    }
}
