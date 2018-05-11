using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UMS;

/// <summary>
/// Responsible for sending data regarding mod files to clients
/// </summary>
public static class ServerModCommunicator {

    public static void Initialize()
    {
        Server.OnClientConnected += ClientConnected;

        Server.EventListener.RegisterCallback((ushort)PackageIdentification.ModDownloadRequest, ReceiveDownloadRequest);
    }
    private static void ClientConnected(Peer peer)
    {
        ModManifestPackage package = new ModManifestPackage(Server.ModManifest);

        peer.SendReliableOrdered(package);
    }
    private static void ReceiveDownloadRequest(Peer peer, byte[] data)
    {
        List<System.Guid> guids = ModDownloadRequest.Process(data);

        foreach (ModFile modFile in Server.LoadedModFiles)
        {
            if(guids.Contains(modFile.GUID))
            {
                Output.Line($"Sending {modFile.FileName} to {peer.ConnectionID}");

                peer.SendReliableOrdered(new ModDownloadPackage(modFile));
            }                
        }
    }
}
