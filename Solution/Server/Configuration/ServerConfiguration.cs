using UnityEngine;
using XML;

namespace Configuration
{
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
        public static int ServerSendRate { get { return Mathf.Clamp(Instance._serverSendRate, 15, 144); } }
        public static int ClientSendRate { get { return Mathf.Clamp(Instance._clientSendRate, 15, 144); } }
        public static string Password { get { return Instance._password; } }
        public static float DespawnTime { get { return Instance._despawnTime; } }
        public static int MaxPingAllowed { get { return Instance._maxPingAllowed; } }
        public static float MaxPacketLossAllowed { get { return Mathf.Clamp(Instance._maxPacketLossAllowed, 0, 1); } }
        public static bool UnconnectedMessagesEnabled { get { return Instance._unconnectedMessagesEnabled; } }
        public static bool NATPunchthrough { get { return Instance._natPunchthroughEnabled; } }
        public static int PingInterval { get { return Instance._pingInterval; } }
        public static int TimeoutTime { get { return Instance._timeoutTime; } }

        [XmlComment("How many clients can be connected concurrently")]
        [XmlElement("MaximumConnections")]
        private int _maximumConnections = 2;

        [XmlComment("Which port to register the server on")]
        [XmlElement("Port")]
        private int _port = 9050;

        [XmlComment("The amount of times per second the server sends data to clients")]
        [XmlElement("ServerSendRate")]
        private int _serverSendRate = 60;

        [XmlComment("The amount of times per second clients send data to the server")]
        [XmlElement("ClientSendRate")]
        private int _clientSendRate = 60;

        [XmlComment("The password required for connecting clients")]
        [XmlComment("NOTE: This is stored in plain text, so DO NOT use it for anything else")]
        [XmlElement("Password")]
        private string _password = string.Empty;

        [XmlComment("Time in seconds it takes for a controlled colonist to return home after a client has disconnected")]
        [XmlElement("DespawnTime")]
        private float _despawnTime = 0;

        [XmlComment("The highest ping allowed before a client is kicked")]
        [XmlElement("MaxPingAllowed")]
        private int _maxPingAllowed = 200;

        [XmlComment("The highest percentage of packet loss allowed before a client is kicked")]
        [XmlComment("This is measured as a percentage, so the value must be between 0 and 1")]
        [XmlElement("MaxPacketLossAllowed")]
        private float _maxPacketLossAllowed = 0.2f;

        [XmlComment("Allow message receiving from clients without connection")]
        [XmlElement("UnconnectedMessagesEnabled")]
        private bool _unconnectedMessagesEnabled = false;

        [XmlComment("Enable NAT punchthrough")]
        [XmlElement("NatPunchthroughEnabled")]
        private bool _natPunchthroughEnabled = false;

        [XmlComment("Interval between latency detection")]
        [XmlComment("Measured in milliseconds")]
        [XmlElement("PingInterval")]
        private int _pingInterval = 1000;

        [XmlComment("Amount of time without response from clients before they're kicked")]
        [XmlComment("Measured in milliseconds")]
        [XmlElement("TimeoutTime")]
        private int _timeoutTime = 5000;
    }
}