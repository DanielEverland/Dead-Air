using Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utility
{
    public const int IPC_PORT = 14768;
    public const int INACTIVE_NETWORK_ID = -1;

    private static System.Random _random = new System.Random();
    
    public static void InitializeNetworkBehaviours(Object obj, int id)
    {
        if (obj is GameObject)
        {
            GameObject gameObject = obj as GameObject;

            foreach (Component comp in gameObject.GetComponents<Component>())
            {
                if (comp is INetworkedObject)
                {
                    INetworkedObject networkObject = comp as INetworkedObject;

                    networkObject.Initialize(id);
                }
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
    public static ulong RandomULong()
    {
        byte[] buffer = new byte[8];
        _random.NextBytes(buffer);
        return System.BitConverter.ToUInt64(buffer, 0);
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