using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Networking
{
    public class PackageEventListener : EventBasedNetListener
    {

        public PackageEventListener()
        {
            _packageCallbacks = new Dictionary<ushort, System.Action<Peer, byte[]>>();

            base.PeerConnectedEvent += PeerConnected;
            base.PeerDisconnectedEvent += PeerDisconnected;
            base.NetworkLatencyUpdateEvent += NetworkLatency;
            base.NetworkReceiveEvent += ReceivePackage;
        }

        public new event System.Action<Peer> PeerConnectedEvent;
        public new event System.Action<Peer, DisconnectInfo> PeerDisconnectedEvent;
        public new event System.Action<Peer, int> NetworkLatencyUpdateEvent;
        public new event System.Action<Peer, NetDataReader> NetworkReceiveEvent;

        private Dictionary<ushort, System.Action<Peer, byte[]>> _packageCallbacks;

        public void RemoveCallback(ushort channelID, System.Action<Peer, byte[]> callback)
        {
            if (!_packageCallbacks.ContainsKey(channelID))
                return;

            _packageCallbacks[channelID] -= callback;
        }
        public void RegisterCallback(ushort channelID, System.Action<Peer, byte[]> callback)
        {
            if (!_packageCallbacks.ContainsKey(channelID))
                _packageCallbacks.Add(channelID, (x, y) => { });

            _packageCallbacks[channelID] += callback;
        }
        private void ReceivePackage(NetPeer peer, NetDataReader reader)
        {
            ushort id = (ushort)(reader.Data[0] + (reader.Data[1] << 8));

            byte[] data = new byte[reader.Data.Length - 2];

            if (reader.Data.Length > 2)
                System.Array.Copy(reader.Data, 2, data, 0, reader.Data.Length - 2);

            if (_packageCallbacks.ContainsKey(id))
            {
                _packageCallbacks[id].Invoke(peer, data);
            }
        }
        private void PeerConnected(NetPeer peer)
        {
            PeerConnectedEvent?.Invoke(peer);
        }
        private void PeerDisconnected(NetPeer peer, DisconnectInfo info)
        {
            PeerDisconnectedEvent?.Invoke(peer, info);
        }
        private void NetworkLatency(NetPeer peer, int latency)
        {
            NetworkLatencyUpdateEvent?.Invoke(peer, latency);
        }
    }
}