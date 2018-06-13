using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Debugging
{
    /// <summary>
    /// Outputting to a cleaner log file
    /// </summary>
    public class OutputLog
    {
        public OutputLog(string fullPath)
        {
            Directories.EnsurePathExists(Path.GetDirectoryName(fullPath));

            //Clear
            File.WriteAllText(fullPath, "");

            _writer = File.AppendText(fullPath);
            _writer.AutoFlush = true;
        }

        public bool EnableTimeStamp { get; set; } = false;
        public StreamWriter Writer { get { return _writer; } }
        
        private StreamWriter _writer;
        
        public void OnReceiveLog(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Line("");
                    Line("[ERROR]");
                    StackTrace(condition, stackTrace);
                    break;
                case LogType.Assert:
                    Line("");
                    Line("[ASSERT]");
                    StackTrace(condition, stackTrace);
                    break;
                case LogType.Warning:
                    Line("");
                    Line("[WARNING]");
                    StackTrace(condition, stackTrace);
                    break;
                case LogType.Log:
                    Line(condition);
                    break;
                case LogType.Exception:
                    Line("");
                    Line("[EXCEPTION]");
                    StackTrace(condition, stackTrace);
                    break;
                default:
                    throw new System.NotImplementedException();
            }            
        }
        public void StackTrace(object message, string stackTrace)
        {
            Line($"{message}\n{stackTrace}");
        }
        public void Line(object message)
        {
            if (EnableTimeStamp)
            {
                message = $"[{DateTime.Now.ToString("HH:mm:ss")}] {message}";
            }

            _writer.WriteLine(message);
        }
    }
}
