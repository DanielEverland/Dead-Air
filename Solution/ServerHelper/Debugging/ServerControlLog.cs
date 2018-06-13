using Debugging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServerControl.Debugging
{
    public static class ServerControlLog
    {
        public static void Initialize()
        {
            if (_hasInitialized)
                return;

            _hasInitialized = true;

            _outputLog = new OutputLog(Directories.ServerHelperLog);
            Console.SetOut(_outputLog.Writer);
            Console.SetError(_outputLog.Writer);

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
        }
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            _outputLog.Line("");
            _outputLog.Line(e.ExceptionObject);
            _outputLog.Line("");
        }

        private static bool _hasInitialized;
        private static OutputLog _outputLog;
    }
}
