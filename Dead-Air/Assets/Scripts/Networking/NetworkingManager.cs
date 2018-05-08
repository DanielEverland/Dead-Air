using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib;

public static class NetworkingManager {

    private static EventBasedNetListener _listener;
    private static NetManager _client;

    public static void Connect(string ipAddress, int port)
    {
        _listener = new EventBasedNetListener();
        _client = new NetManager(_listener, "");

        _client.Start();
        _client.Connect(ipAddress, port);
    }
}
