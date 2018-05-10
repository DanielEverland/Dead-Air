[System.Serializable]
public class ServerConfiguration
{
    public int MaximumConnections { get; set; } = 2;
    public int Port { get; set; } = 9050;
    public int UpdateInterval { get; set; } = 15;
    public string Password { get; set; } = string.Empty;
}
