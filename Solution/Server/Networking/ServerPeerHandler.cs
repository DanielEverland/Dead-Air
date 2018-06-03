using Objects;
using UnityEngine;

namespace Networking
{
    public static class ServerPeerHandler
    {
        public static void SetReady(Peer peer)
        {
            peer.SetReady();
            peer.SendReliableUnordered(new NetworkPackage(PackageIdentification.JoinflowCompleted));

            Profile profile = ProfileManager.GetProfile(peer);

            CreateColonist(peer, profile);
        }
        private static void CreateColonist(Peer peer, Profile profile)
        {
            PeerController controller = PeerController.Get(peer.ProfileID);

            GameObject prefab = ObjectReferenceManifest.GetObject<GameObject>("Colonist");
            GameObject instantiated = Network.Instantiate(prefab, profile.Position, Quaternion.identity);

            instantiated.name = $"Server {peer}";
            instantiated.transform.position = profile.Position;

            controller.AssignColonist(instantiated.GetComponent<Colonist>());
        }
    }
}