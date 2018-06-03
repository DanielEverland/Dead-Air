﻿using System.Collections.Generic;
using System.Linq;
using UMS;

namespace Networking
{
    /// <summary>
    /// Proxy contianer for initializing behaviour on a client
    /// This is simply to avoid cluttering the actual Client class
    /// </summary>
    public static class ClientInitializer
    {
        public static void Initialize()
        {
            ModReceiver.Initialize();
            ClientSpawner.Initialize();
        }
        public static void InitializeObjectReferenceManifest(IEnumerable<ModFile> mods, IDictionary<string, ushort> networkIDs)
        {
            ClientOutput.Line($"Initialize ObjectReferenceManifest with {mods.Count()} mods");

            ObjectReferenceManifest.Initialize(mods, x => ObjectReferenceManifest.ObjectReferenceData.CreateAsClient(x, networkIDs[x.Key]));
        }
    }
}