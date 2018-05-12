using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickstartManager : MonoBehaviour {

    private void Awake()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        SceneManager.LoadScene("Server", LoadSceneMode.Additive);
        SceneManager.LoadScene("Client", LoadSceneMode.Additive);
    }
}
