﻿using System.Linq;
using System.IO;
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
    public static Peer Peer { get; private set; }

    /// <summary>
    /// Does this client have a connection to a server?
    /// </summary>
    public static bool IsConnected { get { return Peer != null; } }

    /// <summary>
    /// Handles receiving of data from other peers
    /// </summary>
    public static PackageEventListener EventListener { get; private set; }

    /// <summary>
    /// The mod files the client has loaded
    /// </summary>
    public static IEnumerable<ModFile> LoadedModFiles { get { return Instance._loadedModfiles; } }

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

    private const string DOWNLOADED_FILES_FOLDER = "Downloaded";

    private NetManager _netManager;
    private List<ModFile> _loadedModfiles;

    public static void Initialize()
    {
        if (global::Server.IsInitialized)
            throw new System.InvalidOperationException("Cannot create a client and a server in the same session");

        Session.Initialize();
        Instance._loadedModfiles = ModLoader.GetAllModFiles();
        
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
    /// <summary>
    /// Add modfile during runtime from server
    /// This will also serialize it to disk
    /// </summary>
    public static void AddModFile(ModFile file)
    {
        if (Instance._loadedModfiles.Contains(file))
            throw new System.InvalidOperationException("Mod file has already been loaded");

        Instance._loadedModfiles.Add(file);

        string folder = $"{Directories.DataPath}/{Settings.ModsDirectory}/{DOWNLOADED_FILES_FOLDER}";
        string fullPath = $"{folder}/{file.FileName}{UMS.Utility.MOD_EXTENSION}";

        Directories.EnsurePathExists(folder);

        Output.Line($"Serializing {file.FileName} to {fullPath}");

        File.WriteAllBytes(fullPath, ByteConverter.Serialize(file));
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
    private static void OnPeerConnected(Peer peer)
    {
        Debug.Log($"Connected to {peer.EndPoint} with ID {peer.ConnectionID}");
    }
    private static void OnPeerDisconnected(Peer peer, DisconnectInfo info)
    {
        Debug.Log($"Disconnected from server with message {info.Reason}");
    }
    private static void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Debug.LogError($"Error ({socketErrorCode}) from {endPoint}");
    }
}
