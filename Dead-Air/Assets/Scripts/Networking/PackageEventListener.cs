using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;

public class PackageEventListener : EventBasedNetListener {

    public PackageEventListener()
    {
        _packageCallbacks = new Dictionary<ushort, System.Action<NetPeer, byte[]>>();

        NetworkReceiveEvent += ReceivePackage;
    }

    private Dictionary<ushort, System.Action<NetPeer, byte[]>> _packageCallbacks;
    
    public void RegisterCallback(ushort channelID, System.Action<NetPeer, byte[]> callback)
    {
        if (!_packageCallbacks.ContainsKey(channelID))
            _packageCallbacks.Add(channelID, (x, y) => { });

        _packageCallbacks[channelID] += callback;
    }
    private void ReceivePackage(NetPeer peer, NetDataReader reader)
    {
        ushort id = (ushort)(reader.Data[0] + (reader.Data[1] << 8));

        byte[] data = new byte[reader.Data.Length - 2];

        if (reader.Data.Length > 2)
            System.Array.Copy(reader.Data, 2, data, 0, reader.Data.Length - 2);

        if (_packageCallbacks.ContainsKey(id))
        {
            _packageCallbacks[id].Invoke(peer, data);
        }
    }
}
