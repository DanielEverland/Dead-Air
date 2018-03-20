using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game {

    public static event System.Action OnHasInitialized;

    public static bool HasInitialized { get { return _hasInitialized; } }
    private static bool _hasInitialized;

    public static void Initialize()
    {
        if (_hasInitialized)
            return;
        
        Mods.Deserialize();        
    }
    public static void OnModsDeserialized()
    {
        LineOfSightManager.Initialize();
        ColonistManager.Initialize();
        MapGenerator.Initialize();
        DayCycle.Initialize();

        _hasInitialized = true;

        if (OnHasInitialized != null)
            OnHasInitialized();
    }
    public static void Update()
    {
        ColonistManager.OnUpdate();
        LineOfSightManager.Update();
        DayCycle.Update();
    }
}
