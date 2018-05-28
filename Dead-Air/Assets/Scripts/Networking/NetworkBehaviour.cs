using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBehaviour : MonoBehaviour, INetworkedObject
{
    public int NetworkID { get { return _networkID; } }
    public bool IsInitialized { get { return _networkID != Utility.INACTIVE_NETWORK_ID; } }
    public bool IsOwned { get { return _isOwned; } }

    private int _networkID = Utility.INACTIVE_NETWORK_ID;
    private bool _isOwned;

    public void Initialize(int id)
    {
        if (_networkID != Utility.INACTIVE_NETWORK_ID)
            throw new System.InvalidOperationException();

        _networkID = id;
        _isOwned = Network.IsOwned(gameObject);
    }
}
