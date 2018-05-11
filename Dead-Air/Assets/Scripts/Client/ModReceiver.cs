using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UMS;

/// <summary>
/// Handles communication with server regarding mods
/// </summary>
public static class ModReceiver {

    public static void Initialize()
    {
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ModManifest, ReceiveModManifest);
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ModDownload, ReceiveModFile);
    }

    private static void ReceiveModManifest(Peer peer, byte[] data)
    {
        List<System.Guid> guids = ModManifestPackage.Process(data);
        IEnumerable<System.Guid> toDownload = guids.Except(Client.LoadedModFiles.Select(x => x.GUID));

        peer.SendReliableOrdered(new ModDownloadRequest(toDownload));
    }
    private static void ReceiveModFile(Peer peer, byte[] data)
    {
        ModFile file = ModDownloadPackage.Process(data);

        Output.Line("Received " + file.FileName);

        Client.AddModFile(file);
    }
}
