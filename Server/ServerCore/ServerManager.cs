using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LiteNetLib;
using ServerCore.Configuration;

namespace ServerCore
{
    public static class ServerManager
    {
        private static ServerConfiguration _configuration;
        private static NetManager _server;
        private static EventBasedNetListener _eventListener;

        public static void Initialize()
        {
            CreateServer();
            SetupEvents();

            Output.Header("Successfully started server");

            while (!Console.KeyAvailable)
            {
                _server.PollEvents();
                Thread.Sleep(_server.UpdateTime);
            }

            Output.Header("Stopping Server");
            _server.Stop();
        }
        private static void CreateServer()
        {
            _configuration = ConfigurationManager.Load<ServerConfiguration>();
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
}
