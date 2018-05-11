using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib;

public static class ModManifestReceiver {

    public static void Initialize()
    {
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ModManifest, ReceivePackage);
    }

    private static void ReceivePackage(Peer peer, byte[] data)
    {
        List<System.Guid> guids = ModManifestPackage.Process(data);
    }
}
