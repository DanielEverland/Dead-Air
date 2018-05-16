using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib;

/// <summary>
/// Wrapper for more direct handling of the peer interface
/// </summary>
public sealed class Peer {

    private Peer(NetPeer peer)
    {
        _peer = peer;
    }

    private static Dictionary<NetPeer, Peer> _cachedPeers = new Dictionary<NetPeer, Peer>();

    public NetStatistics Statistics { get { return _peer.Statistics; } }
    public NetEndPoint EndPoint { get { return _peer.EndPoint; } }
    public ConnectionState ConnectionState { get { return _peer.ConnectionState; } }
    public NetManager Manager { get { return _peer.NetManager; } }

    public event System.Action OnReady;

    /// <summary>
    /// Has the joinflow completed successfully
    /// </summary>
    public bool IsReady { get; private set; }

    public long ConnectionID { get { return _peer.ConnectId; } }
    public int Ping { get { return _peer.Ping; } }
    public int MTU { get { return _peer.Mtu; } }
    public int TimeSinceLastPacket { get { return _peer.TimeSinceLastPacket; } }
    public int PacketCountInReliableQueue { get { return _peer.PacketsCountInReliableOrderedQueue; } }
    public int PacketCountInReliableOrderedQueue { get { return _peer.PacketsCountInReliableOrderedQueue; } }

    private readonly NetPeer _peer;

    public void SetReady()
    {
        if (IsReady)
            return;

        IsReady = true;
        
        OnReady?.Invoke();
    }
    public void Flush()
    {
        _peer.Flush();
    }
    public int GetMaxSinglePacketSize(SendOptions option)
    {
        return _peer.GetMaxSinglePacketSize(option);
    }
    public void SendUnreliable(NetworkPackage package)
    {
        _peer.Send(package, SendOptions.Unreliable);
    }
    public void SendReliableUnordered(NetworkPackage package)
    {
        _peer.Send(package, SendOptions.ReliableUnordered);
    }
    public void SendSequenced(NetworkPackage package)
    {
        _peer.Send(package, SendOptions.Sequenced);
    }
    public void SendReliableOrdered(NetworkPackage package)
    {
        _peer.Send(package, SendOptions.ReliableOrdered);
    }
    public void Send(NetworkPackage package, SendOptions options)
    {
        _peer.Send(package, options);
    }

    private static void Create(NetPeer peer)
    {
        if (_cachedPeers.ContainsKey(peer))
            return;

        Peer newPeer = new Peer(peer);

        _cachedPeers.Add(peer, newPeer);
    }
    public static implicit operator Peer (NetPeer peer)
    {
        if (!_cachedPeers.ContainsKey(peer))
            Create(peer);

        return _cachedPeers[peer];
    }
    public override string ToString()
    {
        return $"{EndPoint} [{ConnectionID}]";
    }
}
