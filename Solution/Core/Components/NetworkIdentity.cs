using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Used to associate a network ID with an object.
    /// Must be on the root object of your prefab
    /// </summary>
    public class NetworkIdentity : MonoBehaviour
    {
        public ulong ID { get; private set; }

        public void AssignID(ulong id)
        {
            ID = id;
        }
    }
}
