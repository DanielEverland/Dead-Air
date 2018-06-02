using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Proxy contianer for initializing behaviour on a client
/// This is simply to avoid cluttering the actual Client class
/// </summary>
public static class ClientInitializer {

    public static void Initialize()
    {
        ModReceiver.Initialize();
    }
}
