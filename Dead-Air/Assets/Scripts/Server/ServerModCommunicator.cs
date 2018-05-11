using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib;

/// <summary>
/// Responsible for sending data regarding mod files to clients
/// </summary>
public static class ServerModCommunicator {

    public static void Initialize()
    {
        Server.OnClientConnected += ClientConnected;
    }
    private static void ClientConnected(Peer peer)
    {
        foreach (System.Guid guid in Server.ModManifest)
        {
            UnityEngine.Debug.Log(guid);
        }

        ModManifestPackage package = new ModManifestPackage(Server.ModManifest);

        peer.Send(package, SendOptions.ReliableOrdered);
    }
}
