using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class CommandLineEventHandler : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private List<string> _commands;
        [SerializeField]
        private UnityEvent _onMatch;
#pragma warning restore

        private void Awake()
        {
            string usedCommands = System.Environment.CommandLine;

            foreach (string command in _commands)
            {
                if (!usedCommands.Contains(command))
                    return;
            }

            _onMatch?.Invoke();
        }
    }
}
