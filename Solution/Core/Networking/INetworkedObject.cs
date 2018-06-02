namespace Networking
{
    /// <summary>
    /// Specifies an object that can be registered across the network
    /// </summary>
    public interface INetworkedObject {

        int NetworkID { get; }

        void Initialize(int id);
    }
}
