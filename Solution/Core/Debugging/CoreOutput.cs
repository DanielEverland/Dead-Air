﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Debugging
{
    public static class CoreOutput
    {
        public static void Initialize()
        {
            if (_hasInitialized)
                return;

            _hasInitialized = true;

            _outputLog = new OutputLog(Directories.OutputLog);
            Application.logMessageReceived += _outputLog.OnReceiveLog;
        }

        private static bool _hasInitialized;
        private static OutputLog _outputLog;
    }
}
