using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientQuickstart : MonoBehaviour {

    private void Start()
    {
        if (!Application.isEditor && !Debug.isDebugBuild)
            throw new System.InvalidOperationException("This can only be run in the editor, or in a debug build");

        Server.Initialize();
        Client.Initialize();

        Client.Connect(new LiteNetLib.NetEndPoint("localhost", ServerConfiguration.Port));
    }
}
