using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickstartManager : MonoBehaviour {

    private void Awake()
    {
        if (!Application.isEditor && !Debug.isDebugBuild)
            throw new System.InvalidOperationException("Cannot run quickstart outside editor or debug build");

        Output.OutputApplicationType = true;

        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        SceneManager.LoadScene("Server", LoadSceneMode.Additive);
        SceneManager.LoadScene("Client", LoadSceneMode.Additive);
    }
}
