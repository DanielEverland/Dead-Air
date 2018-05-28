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
    
    public static bool Initialize()
    {
        try
        {
            if (Client.IsInitialized && !Application.isEditor)
                throw new System.InvalidOperationException("Cannot create a client and a server in the same session");

            Session.Initialize();
            Instance.CreateServer();

            IsInitialized = true;

            ServerInitializer.Initialize();

            ServerOutput.Header("Successfully started server");
            return true;
        }
        catch (System.Exception)
        {
            ServerOutput.HeaderError("Failed starting server");
            
            throw;
        }        
    }
    private void Update()
    {
        JoinFlowManager.Update();

        _netManager.PollEvents();
    }
    public static void SendInstantiationPackage(Object obj, short requestID = -1)
    {
        ushort networkID = ObjectReferenceManifest.GetNetworkID(obj);
        int instanceID = Utility.RandomInt();

        foreach (Peer peer in Network.Peers)
        {
            peer.SendReliableUnordered(new ServerInstantiatePackage(networkID, instanceID, requestID));
        }
    }
    public static void SetReady(Peer peer)
    {
        peer.SetReady();
        peer.SendReliableUnordered(new NetworkPackage(PackageIdentification.JoinflowCompleted));
    }
    private void CreateServer()
    {
        EventListener = new PackageEventListener();
        _netManager = new NetManager(EventListener, ServerConfiguration.MaximumConnections, ServerConfiguration.Password);

        _netManager.UpdateTime = ServerConfiguration.UpdateInterval;
        _netManager.Start(ServerConfiguration.Port);

        ServerOutput.DebugLine($"Max Connections: {ServerConfiguration.MaximumConnections}");
        ServerOutput.DebugLine($"Port: {ServerConfiguration.Port}");
        ServerOutput.DebugLine($"Update Interval: {ServerConfiguration.UpdateInterval}");
        ServerOutput.DebugLine($"Password: {ServerConfiguration.Password}");

        ServerOutput.DebugLine();

        SetupEvents();
        CreateModManifest();
    }
    private void CreateModManifest()
    {
        _modFiles = ModLoader.GetAllModFiles();
        ObjectReferenceManifest.InitializeAsServer(_modFiles);
        ModManifest = new List<System.Guid>(_modFiles.Select(x => x.GUID));
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
        ServerOutput.Line($"Connection {peer.ConnectionID} received from {peer.EndPoint}");

        OnClientConnected?.Invoke(peer);
    }
    private static void OnPeerDisconnected(Peer peer, DisconnectInfo info)
    {
        ServerOutput.Line($"Connection {peer.ConnectionID} disconnected with message {info.Reason}");

        OnClientDisconnected?.Invoke(peer, info);
    }
    private static void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        ServerOutput.Line($"Error ({socketErrorCode}) from {endPoint}");
    }
}
