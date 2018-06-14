using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.UI
{
    public class ChangeScene : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private string _sceneName;
        [SerializeField]
        private LoadSceneMode _mode = LoadSceneMode.Single;
#pragma warning restore

        public void Change()
        {
            SceneManager.LoadScene(_sceneName, _mode);
        }
    }
}