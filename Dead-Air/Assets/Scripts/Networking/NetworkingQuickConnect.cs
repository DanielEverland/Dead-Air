using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingQuickConnect : MonoBehaviour {

    private void Start()
    {
        NetworkingManager.Connect("localhost", 9050);
    }
}
