﻿using Configuration;
using LiteNetLib;
using System.Collections.Generic;
using System.Net;

namespace Networking
{
    /// <summary>
    /// Wrapper for more direct handling of the peer interface
    /// </summary>
    public sealed class Peer : System.IEquatable<Peer>
    {
        private Peer(NetPeer peer)
        {
            _peer = peer;
        }

        private static Dictionary<NetPeer, Peer> _cachedPeers = new Dictionary<NetPeer, Peer>();

        public NetStatistics Statistics { get { return _peer.Statistics; } }
        public IPEndPoint EndPoint { get { return _peer.EndPoint; } }
        public ConnectionState ConnectionState { get { return _peer.ConnectionState; } }
        public NetManager Manager { get { return _peer.NetManager; } }

        public event System.Action OnReady;

        /// <summary>
        /// Has the joinflow completed successfully
        /// </summary>
        public bool IsReady { get; private set; }

        public long ConnectionID { get { return _peer.ConnectId; } }
        public int Ping { get { return _peer.Ping; } }
        public int MTU { get { return _peer.Mtu; } }
        public int TimeSinceLastPacket { get { return _peer.TimeSinceLastPacket; } }
        public int PacketCountInReliableQueue { get { return _peer.PacketsCountInReliableOrderedQueue; } }
        public int PacketCountInReliableOrderedQueue { get { return _peer.PacketsCountInReliableOrderedQueue; } }
        public int ProfileID { get { return _id; } }

        private readonly NetPeer _peer;
        private readonly int _id = LocalData.AccountID;

        public void SetReady()
        {
            if (IsReady)
                return;

            IsReady = true;

            OnReady?.Invoke();
        }
        public void Flush()
        {
            _peer.Flush();
        }
        public int GetMaxSinglePacketSize(DeliveryMethod option)
        {
            return _peer.GetMaxSinglePacketSize(option);
        }
        public void SendUnreliable(NetworkPackage package)
        {
            _peer.Send(package, DeliveryMethod.Unreliable);
        }
        public void SendReliableUnordered(NetworkPackage package)
        {
            _peer.Send(package, DeliveryMethod.ReliableUnordered);
        }
        public void SendSequenced(NetworkPackage package)
        {
            _peer.Send(package, DeliveryMethod.Sequenced);
        }
        public void SendReliableOrdered(NetworkPackage package)
        {
            _peer.Send(package, DeliveryMethod.ReliableOrdered);
        }
        public void Send(NetworkPackage package, DeliveryMethod options)
        {
            _peer.Send(package, options);
        }

        private static void Create(NetPeer peer)
        {
            if (_cachedPeers.ContainsKey(peer))
                return;

            Peer newPeer = new Peer(peer);

            _cachedPeers.Add(peer, newPeer);
        }
        public static implicit operator Peer(NetPeer peer)
        {
            if (!_cachedPeers.ContainsKey(peer))
                Create(peer);

            return _cachedPeers[peer];
        }
        public override int GetHashCode()
        {
            return _peer.EndPoint.GetHashCode();
        }
        public override string ToString()
        {
            return $"{EndPoint} [{ConnectionID}]";
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Peer)
            {
                return Equals(obj as Peer);
            }

            return false;
        }
        public bool Equals(Peer other)
        {
            return other.EndPoint == this.EndPoint;
        }
    }
}