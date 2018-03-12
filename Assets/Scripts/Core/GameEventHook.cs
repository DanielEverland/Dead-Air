using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHook : MonoBehaviour {

    private void Awake()
    {
        Game.Initialize();
    }
    private void Update()
    {
        Game.Update();
    }
}
