using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Assertions;

public static class Network_Initialization
{
    [UnityTest]
    public static IEnumerator Server()
    {
        Assert.IsTrue(Networking.Server.Initialize());

        return null;
    }
    [UnityTest]
    public static IEnumerator Client()
    {
        Assert.IsTrue(Networking.Client.Initialize());

        return null;
    }
}
