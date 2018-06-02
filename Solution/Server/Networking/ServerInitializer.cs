using System.Collections.Generic;
using System.Linq;
using UMS;

namespace Networking
{
    /// <summary>
    /// Proxy contianer for initializing behaviour on a server
    /// This is simply to avoid cluttering the actual Server class
    /// </summary>
    public static class ServerInitializer
    {
        public static void Initialize()
        {
        }
        public static void InitializeObjectReferenceManifest(IEnumerable<ModFile> mods)
        {
            ServerOutput.Line($"Initialize ObjectReferenceManifest with {mods.Count()} mods");

            ObjectReferenceManifest.Initialize(mods, x => ObjectReferenceManifest.ObjectReferenceData.CreateAsServer(x));
        }
    }
}