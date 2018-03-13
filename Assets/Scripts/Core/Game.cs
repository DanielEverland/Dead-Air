using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game {

    private static bool _hasInitialized;

    public static void Initialize()
    {
        if (_hasInitialized)
            return;

        ColonistManager.Initialize();
        MapGenerator.Initialize();

        _hasInitialized = true;
    }
    public static void Update()
    {
        ColonistManager.OnUpdate();
        MapGenerator.Update();
    }
}
