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
        List<System.Guid> toDownload = new List<System.Guid>();
        HashSet<System.Guid> loadedGuids = new HashSet<System.Guid>(Client.LoadedModFiles.Select(x => x.GUID));

        foreach (System.Guid guid in guids)
        {
            if (!loadedGuids.Contains(guid) && !toDownload.Contains(guid))
                toDownload.Add(guid);
        }

        peer.SendReliableOrdered(new ModDownloadRequest(toDownload));
    }
    private static void ReceiveModFile(Peer peer, byte[] data)
    {
        ModFile file = ModDownloadPackage.Process(data);

        Output.Line("Received " + file.FileName);

        Client.AddModFile(file);
    }
}
