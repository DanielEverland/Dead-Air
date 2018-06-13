using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO.Pipes;
using System.IO;
using System.Net;
using LiteNetLib;
using Networking;

namespace ServerControl
{
    public static class IPCManager
    {
        private static NetManager _netManager;
        private static PackageEventListener _listener;

        public static void Initialize()
        {
            System.Console.WriteLine("Creating IPC connection");

            _listener = new PackageEventListener();
            _netManager = new NetManager(_listener);
            _netManager.Start();
            NetPeer peer = _netManager.Connect(new IPEndPoint(IPAddress.Loopback, Utility.IPC_PORT), "");

            System.Console.WriteLine("Connected to " + peer.EndPoint.Port);
        }            
    }
}
