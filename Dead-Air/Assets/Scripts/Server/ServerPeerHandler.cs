using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerPeerHandler {
    
    public static void SetReady(Peer peer)
    {
        peer.SetReady();
        peer.SendReliableUnordered(new NetworkPackage(PackageIdentification.JoinflowCompleted));

        CreateColonist(peer);
    }
    private static void CreateColonist(Peer peer)
    {
        Profile profile = ProfileManager.GetProfile(peer);

        GameObject prefab = ObjectReferenceManifest.GetObject<GameObject>("Colonist");
        GameObject instantiated = Network.Instantiate(prefab);

        instantiated.name = $"Server {peer}";
        instantiated.transform.position = profile.Position;
    }
}
