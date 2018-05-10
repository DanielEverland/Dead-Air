using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

[System.Serializable]
public class ServerConfiguration
{
    public static ServerConfiguration Instance
    {
        get
        {
            if (_instance == null)
                _instance = ConfigurationManager.Load<ServerConfiguration>(Directories.Server);

            return _instance;
        }
    }
    private static ServerConfiguration _instance;
    
    public static int MaximumConnections { get { return Instance._maximumConnections; } }
    public static int Port { get { return Instance._port; } }
    public static int UpdateInterval { get { return Instance._updateInterval; } }
    public static string Password { get { return Instance._password; } }
    
    [XmlComment("How many clients can be connected concurrently")]
    [XmlElement("MaximumConnections")]
    private int _maximumConnections = 2;

    [XmlComment("Which port to register the server on")]
    [XmlElement("Port")]
    private int _port = 9050;

    [XmlComment("The update interval in milliseconds")]
    [XmlElement("UpdateInterval")]
    private int _updateInterval = 15;

    [XmlComment("The password required for connecting clients")]
    [XmlComment("NOTE: This is stored in plain text, so DO NOT use it for anything else")]
    [XmlElement("Password")]
    private string _password = string.Empty;    
}
