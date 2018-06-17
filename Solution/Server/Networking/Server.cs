using Configuration;
using LiteNetLib;
using Modding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UMS;
using System.Net;
using Debugging;
using Controller;

namespace Networking
{
    public class Server
    {
        /// <summary>
        /// Defines whether a server has been initialized
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// GUID of all loaded mod files
        /// </summary>
        public static List<System.Guid> ModManifest { get; private set; }
        
        /// <summary>
        /// The mod files the server has loaded
        /// </summary>
        public static IEnumerable<ModFile> LoadedModFiles { get { return Instance._modFiles; } }

        /// <summary>
        /// Contains information regarding the server which we can send to the client
        /// </summary>
        public static ServerInformation Information { get { return Instance._serverInfo; } }

        public static event System.Action<Peer> OnClientConnected;
        public static event System.Action<Peer, DisconnectInfo> OnClientDisconnected;
        public static event System.Action OnSave;

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

        private ServerInformation _serverInfo;
        private NetManager _netManager;
        private List<ModFile> _modFiles;

        public static bool Initialize()
        {
            try
            {
                ServerLog.Initialize();
                Network.ApplicationQuit += () => { OnSave?.Invoke(); };
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
            ControllerManager.Tick();
            JoinFlowManager.Update();
            PerformanceSender.Update();

            _netManager.PollEvents();
        }
        private void CreateServer()
        {
            //This will start the server helper process
            ControllerManager.Tick();

            _netManager = new NetManager(Network.EventListener, ServerConfiguration.MaximumConnections);

            _netManager.UpdateTime = Mathf.RoundToInt((1 / ServerConfiguration.ServerSendRate) * 100);
            _netManager.UnconnectedMessagesEnabled = ServerConfiguration.UnconnectedMessagesEnabled;
            _netManager.NatPunchEnabled = ServerConfiguration.NATPunchthrough;
            _netManager.PingInterval = ServerConfiguration.PingInterval;
            _netManager.DisconnectTimeout = ServerConfiguration.TimeoutTime;
            
#if DEBUG
            _netManager.SimulateLatency = true;
            _netManager.SimulationMaxLatency = 200;
            _netManager.SimulationMinLatency = 50;

            _netManager.SimulatePacketLoss = true;
            _netManager.SimulationPacketLossChance = 10;
#endif

            _netManager.Start(ServerConfiguration.Port);

            Application.targetFrameRate = ServerConfiguration.ServerSendRate;
            
            SetupEvents();
            CreateModManifest();
            SetupServerInformation();
        }
        private void SetupServerInformation()
        {
            _serverInfo = new ServerInformation(
                ServerConfiguration.ServerSendRate,
                ServerConfiguration.ClientSendRate);
        }
        private void CreateModManifest()
        {
            _modFiles = ModLoader.GetAllModFiles();
            ServerInitializer.InitializeObjectReferenceManifest(_modFiles);
            ModManifest = new List<System.Guid>(_modFiles.Select(x => x.GUID));
        }
        private void SetupEvents()
        {
            Network.EventListener.PeerConnectedEvent += OnPeerConnected;
            Network.EventListener.PeerDisconnectedEvent += OnPeerDisconnected;
            Network.EventListener.NetworkErrorEvent += OnNetworkError;
            Network.EventListener.ConnectionRequestEvent += OnConnectionRequest;
            Network.ApplicationQuit += OnExpectedShutdown;

            Network.RegisterUpdateHandler(Update);
        }
        private static void OnExpectedShutdown()
        {
            System.TimeSpan timeSpan = new System.TimeSpan(0, 0, (int)Time.time);
            
            ServerOutput.Header($"EXPECTED SHUTDOWN AFTER {timeSpan}");

            ControllerManager.Shutdown();
        }
        private static void OnConnectionRequest(ConnectionRequest request)
        {
            request.Accept();
        }
        private static void OnPeerConnected(Peer peer)
        {
            ServerOutput.Line($"Connection {peer.ConnectionID} received from {peer.EndPoint}");

            Network.AddPeer(peer);

            OnClientConnected?.Invoke(peer);
        }
        private static void OnPeerDisconnected(Peer peer, DisconnectInfo info)
        {
            ServerOutput.Line($"Connection {peer.ConnectionID} disconnected with message {info.Reason}");

            Network.RemovePeer(peer);

            OnClientDisconnected?.Invoke(peer, info);
        }
        private static void OnNetworkError(IPEndPoint endPoint, int socketErrorCode)
        {
            ServerOutput.Line($"Error ({socketErrorCode}) from {endPoint}");
        }
    }
}