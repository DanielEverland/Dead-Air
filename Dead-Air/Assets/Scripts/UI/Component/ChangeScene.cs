using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private LoadSceneMode _mode = LoadSceneMode.Single;

    public void OnChange()
    {
        SceneManager.LoadScene(_sceneName, _mode);
    }
}
