using UnityEngine;
using Networking;

namespace Components
{
    public class ClientJoinflowRenderer : MonoBehaviour {

        private void Start()
        {
            Client.Peer.OnReady += OnReady;
        }
        private void OnReady()
        {
            Destroy(gameObject);
        }
    }
}
