using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Utility {

    public static List<System.Guid> ByteArrayToGUID(byte[] data)
    {
        List<System.Guid> toReturn = new List<System.Guid>();

        for (int i = 0; i < data.Length / 16; i++)
        {
            int index = i * 16;
            byte[] guidData = new byte[16];

            for (int j = 0; j < 16; j++)
            {
                guidData[j] = data[index + j];
            }

            toReturn.Add(new System.Guid(guidData));
        }

        return toReturn;
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
