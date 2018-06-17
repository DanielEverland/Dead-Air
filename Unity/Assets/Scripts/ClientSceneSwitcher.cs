using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;
using UnityEngine.SceneManagement;

public class ClientSceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private string _toLoad = "Client Main";
    [SerializeField]
    private string _toUnload = "Client Joining";

	public void Update ()
    {
        if (Client.Peer.IsReady)
        {
            SceneManager.UnloadSceneAsync(_toUnload);
            SceneManager.LoadScene(_toLoad, LoadSceneMode.Additive);
            
            Destroy(this);
        }
	}
}
