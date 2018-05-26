using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Utility {
    
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
