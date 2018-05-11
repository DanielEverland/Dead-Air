using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using UMS;

public class Server {
    
    /// <summary>
    /// Defines whether a server has been initialized
    /// </summary>
    public static bool IsInitialized { get; private set; }

    public static event System.Action OnClientConnected;
    public static event System.Action OnClientDisconnected;

    private static Server Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Server();

            return _instance;
        }
    }
    private static Server _instance;

    private ServerConfiguration _configuration;
    private NetManager _netManager;
    private EventBasedNetListener _eventListener;
    private List<ModFile> _modFiles;
    private List<System.Guid> _modManifest;
    
    public static void Initialize()
    {
        if (Client.IsInitialized)
            throw new System.InvalidOperationException("Cannot create a client and a server in the same session");

        Instance.CreateServer();

        IsInitialized = true;

        Output.Header("Successfully started server");
    }
    private void Update()
    {
        _netManager.PollEvents();
    }    
    private void CreateServer()
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

        SetupEvents();
        CreateModManifest();
    }
    private void CreateModManifest()
    {
        _modFiles = ModLoader.GetAllModFiles();
        _modManifest = new List<System.Guid>();

        foreach (ModFile file in _modFiles)
        {
            _modManifest.Add(file.GUID);
        }

        ServerModCommunicator.Initialize();
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
        Output.Line($"Connection {peer.ConnectId} received from {peer.EndPoint}");

        OnClientConnected?.Invoke();
    }
    private static void OnPeerDisconnected(NetPeer peer, DisconnectInfo info)
    {
        Output.Line($"Connection {peer.ConnectId} disconnected with message {info.Reason}");

        OnClientDisconnected?.Invoke();
    }
    private static void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Output.Line($"Error ({socketErrorCode}) from {endPoint}");
    }
}
