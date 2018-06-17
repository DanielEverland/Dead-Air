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
        public ServerPerformance(int frameRate)
        {
            _frameRate = frameRate;
        }

        public int FrameRate { get { return _frameRate; } }

        [ProtoMember(1)]
        private int _frameRate;
    }
}
