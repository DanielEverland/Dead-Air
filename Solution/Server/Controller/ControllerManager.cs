using System.IO.Pipes;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LiteNetLib;
using Networking;

namespace Controller
{
    /// <summary>
    /// Manages communcation and runtime of the server helper process
    /// </summary>
    internal static class ControllerManager
    {
        private static Process _process;
        private static NetManager _netManager;
        private static PackageEventListener _listener;

        static ControllerManager()
        {
            _listener = new PackageEventListener();
            _netManager = new NetManager(_listener);
            _netManager.Start(Utility.IPC_PORT);

            ServerOutput.Line($"Started IPC server on {Utility.IPC_PORT}");

            CreateProcess();
        }
        public static void Tick()
        {
            PollProcess();
        }
        private static void PollProcess()
        {
            if(_process == null)
            {
                ServerOutput.HeaderError("Unexpected shutdown of controller");

                if (Application.isEditor)
                {
                    Hooks.StopPlaying();
                }
                else
                {
                    Application.Quit();
                }
            }
            else if(_process.HasExited)
            {
                _process = null;
            }
        }
        private static void CreateProcess()
        {
            if (_process != null)
                return;
            
            _process = new Process();
            _process.StartInfo.FileName = Directories.ServerHelper;
            _process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            _process.Start();
        }
        public static void Shutdown()
        {
            if(!_process.HasExited)
                _process.Kill();
        }
    }
}
