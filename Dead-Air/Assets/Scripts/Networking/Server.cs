using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;

public class Server : MonoBehaviour {

    private static ServerConfiguration _configuration;
    private static NetManager _server;
    private static EventBasedNetListener _eventListener;

    private static bool _isInitialized;
    private const string SCENE_NAME = "Server";

    private void Update()
    {
        if (!_isInitialized)
            return;

        _server.PollEvents();
    }
    public static void Initialize()
    {
        SceneManager.LoadScene(SCENE_NAME, LoadSceneMode.Additive);

        CreateServer();
        SetupEvents();

        Output.Header("Successfully started server");

        _isInitialized = true;
    }
    private static void CreateServer()
    {
        _configuration = ConfigurationManager.Load<ServerConfiguration>(Directories.Server);
        _eventListener = new EventBasedNetListener();
        _server = new NetManager(_eventListener, _configuration.MaximumConnections, _configuration.Password);

        _server.UpdateTime = _configuration.UpdateInterval;
        _server.Start(_configuration.Port);

        Output.DebugLine($"Max Connections: {_configuration.MaximumConnections}");
        Output.DebugLine($"Port: {_configuration.Port}");
        Output.DebugLine($"Update Interval: {_configuration.UpdateInterval}");
        Output.DebugLine($"Password: {_configuration.Password}");

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
