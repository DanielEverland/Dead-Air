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

            _server.Stop();
        }
        private static void CreateServer()
        {
            _configuration = ConfigurationManager.Load<ServerConfiguration>();
            _eventListener = new EventBasedNetListener();
            _server = new NetManager(_eventListener, _configuration.MaximumConnections, "");

            _server.UpdateTime = _configuration.UpdateInterval;
            _server.Start(_configuration.Port);

            Output.DebugLine("Outputting Configuration");
            Output.DebugLine($"Max Connections: {_configuration.MaximumConnections}");
            Output.DebugLine($"Port: {_configuration.Port}");
            Output.DebugLine($"Update Interval: {_configuration.UpdateInterval}");

            Output.DebugLine();
        }
        private static void SetupEvents()
        {

        }
    }
}
