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

    /// <summary>
    /// GUID of all loaded mod files
    /// </summary>
    public static List<System.Guid> ModManifest { get; private set; }

    /// <summary>
    /// Handles receiving of data from other peers
    /// </summary>
    public static PackageEventListener EventListener { get; private set; }

    /// <summary>
    /// The mod files the server has loaded
    /// </summary>
    public static IEnumerable<ModFile> LoadedModFiles { get { return Instance._modFiles; } }

    public static event System.Action<Peer> OnClientConnected;
    public static event System.Action<Peer, DisconnectInfo> OnClientDisconnected;

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

    private NetManager _netManager;
    private List<ModFile> _modFiles;
    
    public static void Initialize()
    {
        if (Client.IsInitialized)
            throw new System.InvalidOperationException("Cannot create a client and a server in the same session");

        Session.Initialize();
        Instance.CreateServer();

        IsInitialized = true;

        ServerInitializer.Initialize();

        Output.Header("Successfully started server");
    }
    private void Update()
    {
        _netManager.PollEvents();
    }    
    private void CreateServer()
    {
        EventListener = new PackageEventListener();
        _netManager = new NetManager(EventListener, ServerConfiguration.MaximumConnections, ServerConfiguration.Password);

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
        ModManifest = new List<System.Guid>();

        foreach (ModFile file in _modFiles)
        {
            ModManifest.Add(file.GUID);
        }

        ServerModCommunicator.Initialize();
    }
    private void SetupEvents()
    {
        EventListener.PeerConnectedEvent += OnPeerConnected;
        EventListener.PeerDisconnectedEvent += OnPeerDisconnected;
        EventListener.NetworkErrorEvent += OnNetworkError;

        Network.RegisterUpdateHandler(Update);
    }
    private static void OnPeerConnected(Peer peer)
    {
        Output.Line($"Connection {peer.ConnectionID} received from {peer.EndPoint}");

        OnClientConnected?.Invoke(peer);
    }
    private static void OnPeerDisconnected(Peer peer, DisconnectInfo info)
    {
        Output.Line($"Connection {peer.ConnectionID} disconnected with message {info.Reason}");

        OnClientDisconnected?.Invoke(peer, info);
    }
    private static void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Output.Line($"Error ({socketErrorCode}) from {endPoint}");
    }
}
