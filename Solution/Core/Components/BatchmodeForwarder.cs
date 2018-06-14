using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components
{
    public class BatchmodeForwarder : MonoBehaviour
    {
        [SerializeField]
        private string targetScene;
        [SerializeField]
        private LoadSceneMode loadMode = LoadSceneMode.Single;

        private void Awake()
        {
            string commands = System.Environment.CommandLine;

            if(commands.Contains("-batchmode") && commands.Contains("-nographics"))
            {
                SceneManager.LoadScene(targetScene, loadMode);
            }
        }
    }
}
