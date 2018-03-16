using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS.Deserialization;

public static class Game {

    public static bool HasInitialized { get { return _hasInitialized; } }
    private static bool _hasInitialized;

    public static void Initialize()
    {
        if (_hasInitialized)
            return;

        Deserializer.OnHasInitialized += OnModdingHasLoaded;
        Deserializer.Initialize();
    }
    private static void OnModdingHasLoaded()
    {
        LineOfSightManager.Initialize();
        ColonistManager.Initialize();
        MapGenerator.Initialize();
        DayCycle.Initialize();

        _hasInitialized = true;
    }
    public static void Update()
    {
        ColonistManager.OnUpdate();
        LineOfSightManager.Update();
        DayCycle.Update();
    }
}
