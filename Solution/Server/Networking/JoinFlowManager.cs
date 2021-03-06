﻿using System.Collections.Generic;

namespace Networking
{
    public static class JoinFlowManager
    {
        static JoinFlowManager()
        {
            _activeJoinFlows = new Dictionary<Peer, JoinFlow>();
            _removalQueue = new Queue<Peer>();
            _addedPeers = new HashSet<Peer>();

            Server.OnClientConnected += AddPeer;
            Network.EventListener.RegisterCallback((ushort)PackageIdentification.ModDownloadRequest, ReceiveDownloadRequest);
            Network.EventListener.RegisterCallback((ushort)PackageIdentification.RequestObjectIDManifest, RequestObjectIDManifest);
        }

        private static Dictionary<Peer, JoinFlow> _activeJoinFlows;
        private static Queue<Peer> _removalQueue;

        private static HashSet<Peer> _addedPeers;

        public static void Update()
        {
            while (_removalQueue.Count > 0)
            {
                _activeJoinFlows.Remove(_removalQueue.Dequeue());
            }

            foreach (KeyValuePair<Peer, JoinFlow> pair in _activeJoinFlows)
            {
                pair.Value.Update();
            }
        }
        private static void ReceiveDownloadRequest(Peer peer, byte[] data)
        {
            List<System.Guid> toDownload = data.Deserialize<List<System.Guid>>();

            Get(peer).ReceiveDownloadRequest(toDownload);
        }
        private static void RequestObjectIDManifest(Peer peer, byte[] data)
        {
            Get(peer).ReceiveObjectIDManifestRequest();
        }
        public static void AddPeer(Peer peer)
        {
            if (_addedPeers.Contains(peer))
                throw new System.InvalidOperationException("Client " + peer + " has already been added");

            _addedPeers.Add(peer);
            _activeJoinFlows.Add(peer, new JoinFlow(peer));
        }
        public static void Remove(JoinFlow flow)
        {
            if (!_removalQueue.Contains(flow.Peer))
            {
                _removalQueue.Enqueue(flow.Peer);

                ServerOutput.Header($"Finished joinflow for {flow.Peer}");
            }
        }
        private static JoinFlow Get(Peer peer)
        {
            if (!_activeJoinFlows.ContainsKey(peer))
                throw new System.NullReferenceException("No active joinflow for " + peer);

            return _activeJoinFlows[peer];
        }
    }
}