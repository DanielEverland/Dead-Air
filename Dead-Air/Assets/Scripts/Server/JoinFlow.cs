using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS;

public class JoinFlow {

    public JoinFlow(Peer peer)
    {
        _peer = peer;

        Output.Header($"Starting joinflow for {peer}");
    }

    public Peer Peer { get { return _peer; } }
    
    private readonly Peer _peer;
    private State _state;

    public void Update()
    {
        switch (_state)
        {
            case State.SendModManifest:
                SendModManfiest();
                break;
            case State.SendObjectManifest:
                SendObjectManifest();
                break;
            case State.Finish:
                JoinFlowManager.Remove(this);
                break;
        }
    }
    private void SendModManfiest()
    {
        _peer.SendReliableOrdered(new NetworkPackage(PackageIdentification.ModManifest, Server.ModManifest));

        SwitchState(State.WaitForModResponse);
    }
    public void ReceiveDownloadRequest(List<System.Guid> toDownload)
    {
        foreach (ModFile modFile in Server.LoadedModFiles)
        {
            if (toDownload.Contains(modFile.GUID))
            {
                _peer.SendReliableOrdered(new ModDownloadPackage(modFile));
            }
        }

        SwitchState(State.SendObjectManifest);
    }
    public void ReceiveObjectIDManifestRequest()
    {
        SwitchState(State.SendObjectManifest);
    }
    private void SendObjectManifest()
    {
        _peer.SendReliableOrdered(new NetworkPackage(PackageIdentification.ObjectIDManifest, ObjectReferenceManifest.GetAllNetworkIDs()));

        SwitchState(State.Finish);
    }
    private void SwitchState(State newState)
    {
        Output.Line($"{_peer}: {newState}");

        _state = newState;
    }

    private enum State
    {
        SendModManifest,
        WaitForModResponse,
        SendObjectManifest,
        Finish,
    }
}
