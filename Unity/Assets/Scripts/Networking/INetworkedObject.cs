using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specifies an object that can be registered across the network
/// </summary>
public interface INetworkedObject {

	int NetworkID { get; }

    void Initialize(int id);
}
