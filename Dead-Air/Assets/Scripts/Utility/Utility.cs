using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utility {

    public const int INACTIVE_NETWORK_ID = -1;

    private static System.Random _random = new System.Random();

    public static void InitializeNetworkBehaviours(GameObject obj, int id)
    {
        foreach (Component comp in obj.GetComponents<Component>())
        {
            if(comp is INetworkedObject)
            {
                INetworkedObject networkObject = comp as INetworkedObject;

                networkObject.Initialize(id);
            }
        }
    }
    public static int RandomInt()
    {
        return _random.Next();
    }
    public static short RandomShort()
    {
        byte[] randomBytes = new byte[2];
        _random.NextBytes(randomBytes);

        return (short)(randomBytes[0] + (randomBytes[1] << 8));
    }
    public static bool SceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name)
                return true;
        }

        return false;
    }
}
