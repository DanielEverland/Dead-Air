using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networking;

namespace Metrics
{
    public static class NetworkPerformance
    {
        public static Statistics Update()
        {
            return new Statistics()
            {
                Ping = Client.Peer.Ping,
            };
        }

        public struct Statistics
        {
            public int Ping;
        }
    }
}
