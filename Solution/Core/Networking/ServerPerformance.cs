using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ProtoBuf;

namespace Networking
{
    /// <summary>
    /// Contains information regarding the servers performance
    /// </summary>
    [ProtoContract]
    public class ServerPerformance
    {
        public ServerPerformance() { }
        public ServerPerformance(int frameRate, float packetLoss)
        {
            _frameRate = frameRate;
            _packetLoss = Mathf.Clamp(packetLoss, 0, 100);
        }

        public static readonly ServerPerformance None = new ServerPerformance(0, 0);

        public int FrameRate { get { return _frameRate; } }
        public float PacketLoss { get { return _packetLoss; } }

        [ProtoMember(1)]
        private int _frameRate;
        [ProtoMember(2)]
        private float _packetLoss;
    }
}
