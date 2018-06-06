using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Debugging
{
    public static class ServerLog
    {
        public static void Initialize()
        {
            if (_hasInitialized)
                return;

            _hasInitialized = true;

            _outputLog = new OutputLog(Directories.ServerLog);
            _outputLog.EnableTimeStamp = true;
        }

        private static bool _hasInitialized;
        private static OutputLog _outputLog;
    }
}
