using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Networking;
using Configuration;
using System.Net;

public class NetworkQuickstart : MonoBehaviour {

    [SerializeField]
    private InitializationState _networkType;

    private Scene _mainScene;

    private void Awake()
    {
        if (!Utility.SceneLoaded("Main"))
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);
            _mainScene = SceneManager.GetSceneByName("Main");
        }            
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
            Client.Connect(new IPEndPoint(IPAddress.Loopback, ServerConfiguration.Port));
        }
    }
    private void Update()
    {
        if (_mainScene.isLoaded)
        {
            SceneManager.SetActiveScene(_mainScene);
            Destroy(this);
        }
    }

    private enum InitializationState
    {
        None = 0,

        Server = 1,
        Client = 2,
    }
}
