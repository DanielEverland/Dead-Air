using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
