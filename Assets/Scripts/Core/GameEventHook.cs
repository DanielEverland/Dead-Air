using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHook : MonoBehaviour {

    private void Start()
    {
        Game.Initialize();
    }
    private void Update()
    {
        if(Game.HasInitialized)
            Game.Update();
    }
}
