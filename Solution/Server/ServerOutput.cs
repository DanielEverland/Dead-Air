public static class ServerOutput {

    private static string Prefix
    {
        get
        {
            if (Output.OutputApplicationType)
            {
                return PREFIX;
            }

            return string.Empty;
        }
    }
    private const string PREFIX = "[SERVER] ";

    public static void DebugLine() { Output.DebugLine(); }
    public static void Line() { Output.Line(); }

    public static void DebugLine(object message)
    {
        Output.DebugLine(Prefix + message);
    }
    public static void DebugHeader(object message)
    {
        Output.DebugHeader(Prefix + message);
    }
    public static void LineError(object message)
    {
        Output.LineError(Prefix + message);
    }
    public static void Line(object message)
    {
        Output.Line(Prefix + message);
    }
    public static void Header(object message)
    {
        Output.Header(Prefix + message);
    }
    public static void HeaderError(object message)
    {
        Output.HeaderError(Prefix + message);
    }
}
