using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class TransformSync : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private NetworkIdentity _identity;
#pragma warning restore

        private void Start()
        {
            Debug.Log(_identity.ID);
        }
    }
}
