using System.Collections.Generic;
using UMS;

namespace Networking
{
    /// <summary>
    /// Joinflow handler on server
    /// </summary>
    public class JoinFlow
    {
        public JoinFlow(Peer peer)
        {
            _peer = peer;

            ServerOutput.Header($"Starting joinflow for {peer}");

            PeerController.Create(peer);
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
                    Finish();
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
            ServerOutput.Line($"{Peer}: Download Request For {toDownload.Count} Mods");

            foreach (ModFile modFile in Server.LoadedModFiles)
            {
                if (toDownload.Contains(modFile.GUID))
                {
                    ServerOutput.Line($"Sending {modFile.GUID}");

                    _peer.SendReliableOrdered(new NetworkPackage(PackageIdentification.ModDownload, modFile));

                    toDownload.Remove(modFile.GUID);
                }
            }

            //This means the client has requested a mod we don't have
            if (toDownload.Count > 0)
            {
                ServerOutput.LineError($"{Peer}: Requested {toDownload.Count} mods we don't posess\n\n{string.Join("\n", toDownload)}\n");
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
        private void Finish()
        {
            JoinFlowManager.Remove(this);
            ServerPeerHandler.SetReady(Peer);
        }
        private void SwitchState(State newState)
        {
            ServerOutput.Line($"{_peer}: {newState}");

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
}