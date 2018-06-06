using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public static class Network_Initialization
{
    [UnityTest]
    public static IEnumerator Server()
    {
        SceneManager.LoadScene("Server", LoadSceneMode.Single);

        yield return null;
    }
    [UnityTest]
    public static IEnumerator Client()
    {
        SceneManager.LoadScene("Client", LoadSceneMode.Single);

        yield return null;
    }
}
