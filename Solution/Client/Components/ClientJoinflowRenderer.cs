using Networking;
using UnityEngine;

namespace Components
{
    public class ClientJoinflowRenderer : MonoBehaviour
    {
        private void Start()
        {
            Client.OnReady += OnReady;
        }
        private void OnReady()
        {
            Destroy(gameObject);
        }
    }
}