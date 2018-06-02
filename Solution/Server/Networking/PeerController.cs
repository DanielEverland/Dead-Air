using System.Collections.Generic;
using UnityEngine;
using Networking;
using Objects;

namespace Networking {

    /// <summary>
    /// Handles all functions and data related to a peer
    /// </summary>
    public class PeerController
    {

        private PeerController(Peer peer)
        {
            _controllers.Add(peer.ProfileID, this);
        }

        private static Dictionary<int, PeerController> _controllers = new Dictionary<int, PeerController>();

        public static PeerController Create(Peer peer)
        {
            return new PeerController(peer);
        }
        public static PeerController Get(int profileID)
        {
            try
            {
                return _controllers[profileID];
            }
            catch (System.Exception)
            {
                Debug.LogError("Tried to get controller with profile ID " + profileID);
                throw;
            }
        }

        public Colonist Colonist { get; private set; }

        private readonly Peer _peer;

        public void AssignColonist(Colonist colonist)
        {
            Colonist = colonist;
        }
    } 
}