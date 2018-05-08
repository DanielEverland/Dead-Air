using System;

namespace ServerCore
{
    public static class Output
    {
        private const string HEADER_FORMAT = "========  {0} ========";

#if DEBUG
        private const bool DEBUG = true;
#else
        private const bool DEBUG = false;
#endif

#pragma warning disable 0162
        public static void WriteDebug(object message)
        {
            if(DEBUG)
                Console.Write(message);
        }
        public static void DebugLine()
        {
            if (DEBUG)
                Console.WriteLine();
        }
        public static void DebugLine(object message)
        {
            if (DEBUG)
                Console.WriteLine(message);
        }
        public static void DebugHeader(object message)
        {
            if (DEBUG)
                Console.WriteLine(string.Format(HEADER_FORMAT, message));
        }
#pragma warning restore

        public static void Write(object message)
        {
            Console.Write(message);
        }
        public static void Line()
        {
            Console.WriteLine();
        }
        public static void Line(object message)
        {
            Console.WriteLine(message);
        }
        public static void Header(object message)
        {
            Console.WriteLine(string.Format(HEADER_FORMAT, message));
        }
    }
}
