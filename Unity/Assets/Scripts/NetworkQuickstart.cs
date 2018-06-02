using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Networking;
using Configuration;

public class NetworkQuickstart : MonoBehaviour {

    [SerializeField]
    private InitializationState _networkType;

    private void Awake()
    {
        if (!Utility.SceneLoaded("Main"))
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);   
    }
    private void Start()
    {
        if (_networkType == InitializationState.Server)
        {
            Server.Initialize();
        }
        else if (_networkType == InitializationState.Client)
        {
            Client.Initialize();
            Client.Connect(new LiteNetLib.NetEndPoint("localhost", ServerConfiguration.Port));
        }
    }

    private enum InitializationState
    {
        None = 0,

        Server = 1,
        Client = 2,
    }
}
