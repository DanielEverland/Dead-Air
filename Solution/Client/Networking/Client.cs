﻿using LiteNetLib;
using Modding;
using Serialization;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UMS;

namespace Networking
{
    public class Client
    {
        /// <summary>
        /// Raised when the client has completed joinflow and is ready for use
        /// </summary>
        public static event System.Action OnReady;


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
        /// The mod files the client has loaded
        /// </summary>
        public static IEnumerable<ModFile> LoadedModFiles { get { return Instance._loadedModfiles; } }

        /// <summary>
        /// The net manager for the client
        /// </summary>
        public static NetManager Manager { get { return Instance._netManager; } }

        /// <summary>
        /// Information regarding the server we're connected to
        /// </summary>
        public static ServerInformation ServerInformation { get { return Instance._serverInfo; } }
        
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

        private ServerInformation _serverInfo;
        private NetManager _netManager;
        private List<ModFile> _loadedModfiles;

        public static bool Initialize()
        {
            try
            {
                Session.Initialize();
                Instance._loadedModfiles = ModLoader.GetAllModFiles();

                Instance.CreateClient();
                Instance.SetupEvents();

                ClientInitializer.Initialize();
                return true;
            }
            catch (System.Exception)
            {
                ClientOutput.HeaderError("Failed starting client");
                throw;
            }            
        }
        public static void Connect(IPEndPoint endpoint)
        {
            if (IsConnected)
                throw new System.InvalidOperationException("We already have an established connection to the server");

            ClientOutput.Line("Connecting to " + endpoint);

            Instance._netManager.Start();

            Network.EventListener.RegisterCallback((ushort)PackageIdentification.JoinflowCompleted, SetReady);

            Peer = Instance._netManager.Connect(endpoint, "");
            Peer.OnReady += Ready;
        }
        private static void SetReady(Peer peer, byte[] data)
        {
            Network.EventListener.RemoveCallback((ushort)PackageIdentification.JoinflowCompleted, SetReady);

            ClientOutput.Line("Client is ready, joinflow complete");

            Peer.SetReady();
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
            string fullPath = $"{folder}/{file.Name}{UMS.Utility.MOD_EXTENSION}";

            Directories.EnsurePathExists(folder);

            ClientOutput.Line($"Serializing {file} to {fullPath}");

            File.WriteAllBytes(fullPath, ByteConverter.Serialize(file));
        }
        public static void UpdateServerInformation(ServerInformation info)
        {
            Instance._serverInfo = info;
        }

        private void Update()
        {
            _netManager.PollEvents();
        }
        private void CreateClient()
        {
            _netManager = new NetManager(Network.EventListener);
        }
        private void SetupEvents()
        {
            Network.EventListener.PeerConnectedEvent += OnPeerConnected;
            Network.EventListener.PeerDisconnectedEvent += OnPeerDisconnected;
            Network.EventListener.NetworkErrorEvent += OnNetworkError;

            Network.RegisterUpdateHandler(Update);
        }
        private static void OnPeerConnected(Peer peer)
        {
            ClientOutput.Line($"Connected to {peer.EndPoint} with ID {peer.ConnectionID}");
        }
        private static void OnPeerDisconnected(Peer peer, DisconnectInfo info)
        {
            ClientOutput.Line($"Disconnected from server with message {info.Reason}");
        }
        private static void OnNetworkError(IPEndPoint endPoint, int socketErrorCode)
        {
            ClientOutput.LineError($"Error ({socketErrorCode}) from {endPoint}");
        }
        private static void Ready()
        {
            OnReady?.Invoke();
            OnReady = null;
        }
    }
}