using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networking;

namespace Metrics
{
    public static class ServerPerformanceReceiver
    {
        public static void Initialize()
        {
            Networking.Network.EventListener.RegisterCallback((ushort)PackageIdentification.ServerPerformance, ReceivePackage);
        }
        private static void ReceivePackage(Peer peer, byte[] data)
        {
            ServerPerformance package = data.Deserialize<ServerPerformance>();

            Client.UpdateServerPerformance(package);
        }
    }
}
