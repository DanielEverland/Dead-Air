using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Components
{
    public class ChangeScene : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private string _sceneName;
        [SerializeField]
        private LoadSceneMode _mode = LoadSceneMode.Single;
#pragma warning restore

        public void OnChange()
        {
            SceneManager.LoadScene(_sceneName, _mode);
        }
    }
}