using UnityEngine;

public static class Output
{
    /// <summary>
    /// Should we display whether the current application is CLIENT or SERVER?
    /// This is usually only used in-editor
    /// </summary>
    public static bool OutputApplicationType { get; set; } = false;

    private const string HEADER_FORMAT = "========  {0} ========";

    private static bool IsDebug
    {
        get
        {
            return Application.isEditor || Debug.isDebugBuild;
        }
    }

    public static void DebugLine()
    {
        if (IsDebug)
            Debug.Log("");
    }
    public static void DebugLine(object message)
    {
        if (IsDebug)
            Debug.Log(message);
    }
    public static void DebugHeader(object message)
    {
        if (IsDebug)
            Debug.Log(string.Format(HEADER_FORMAT, message));
    }
    public static void Line()
    {
        Debug.Log("");
    }
    public static void LineError(object message)
    {
        Debug.LogError(message);
    }
    public static void Line(object message)
    {
        Debug.Log(message);
    }
    public static void Header(object message)
    {
        Debug.Log(string.Format(HEADER_FORMAT, message));
    }
    public static void HeaderError(object message)
    {
        Debug.LogError(string.Format(HEADER_FORMAT, message));
    }
}