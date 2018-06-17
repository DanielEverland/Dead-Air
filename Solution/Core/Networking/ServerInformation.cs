using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ProtoBuf;

namespace Networking
{
    /// <summary>
    /// Contains information regarding a server that can be shared with clients
    /// </summary>
    [ProtoContract]
    public class ServerInformation
    {
        public ServerInformation() { }
        public ServerInformation(int serverSendRate, int clientSendRate)
        {
            _serverSendRate = serverSendRate;
            _clientSendRate = clientSendRate;
        }

        public int ServerSendRate { get { return _serverSendRate; } }
        public int ClientSendRate { get { return _clientSendRate; } }

        [ProtoMember(1)]
        private int _serverSendRate;
        [ProtoMember(2)]
        private int _clientSendRate;
    }
}
