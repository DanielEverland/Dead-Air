using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LiteNetLib;

namespace ServerApplication
{
    public static class ServerManager
    {
        /// <summary>
        /// Max amount of connections to allow
        /// </summary>
        private const int MAX_CONNECTIONS = 2;

        /// <summary>
        /// The port used
        /// </summary>
        private const int PORT = 9050;

        /// <summary>
        /// The amount of milliseconds between every update
        /// </summary>
        private const int UPDATE_INTERVAL = 16;

        private static NetManager _server;
        private static EventBasedNetListener _eventListener;

        public static void Initialize()
        {
            CreateServer();
            SetupEvents();

            Console.WriteLine("====== Successfully started server ======");
            
            while (!Console.KeyAvailable)
            {
                _server.PollEvents();
                Thread.Sleep(_server.UpdateTime);
            }

            _server.Stop();
        }
        private static void CreateServer()
        {
            _eventListener = new EventBasedNetListener();
            _server = new NetManager(_eventListener, MAX_CONNECTIONS, "");

            _server.UpdateTime = UPDATE_INTERVAL;
            _server.Start(PORT);

#if DEBUG
            Console.WriteLine("Outputting Configuration");
            Console.WriteLine($"Max Connections: {MAX_CONNECTIONS}");
            Console.WriteLine($"Port: {PORT}");
            Console.WriteLine($"Update Interval: {UPDATE_INTERVAL}");

            Console.WriteLine();
#endif
        }
        private static void SetupEvents()
        {

        }
    }
}
