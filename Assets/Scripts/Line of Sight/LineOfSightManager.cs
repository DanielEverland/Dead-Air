using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineOfSightManager {
    
    public static bool HasInitialized { get; private set; }

    private static List<LineOfSightActor> _actors;

    /// <summary>
    /// How many times a second should we update the texture
    /// </summary>
    private const float REFRESH_RATE = 30;

    private static float _timeSinceLastUpdate;
    private static HashSet<Vector2Int> _passiveCells;
    private static HashSet<Vector2Int> _activeCells;

    public static void Initialize()
    {
        _actors = new List<LineOfSightActor>();
        _passiveCells = new HashSet<Vector2Int>();
        _activeCells = new HashSet<Vector2Int>();
    }
    public static void AddActor(LineOfSightActor actor)
    {
        if (!_actors.Contains(actor))
            _actors.Add(actor);
    }
    public static void RemoveActor(LineOfSightActor actor)
    {
        if (_actors.Contains(actor))
            _actors.Remove(actor);
    }
    public static LineOfSightState GetState(Vector2Int worldPosition)
    {
        if (_activeCells.Contains(worldPosition))
        {
            return LineOfSightState.Active;
        }
        else if (_passiveCells.Contains(worldPosition))
        {
            return LineOfSightState.Passive;
        }
        else
        {
            return LineOfSightState.Disabled;
        }
    } 
    public static void Update()
    {
        _timeSinceLastUpdate += Time.unscaledDeltaTime;

        if(_timeSinceLastUpdate > 1 / REFRESH_RATE)
        {
            _timeSinceLastUpdate = 0;

            Poll();
            LineOfSightRenderer.Render();
        }
    }
    private static void Poll()
    {
        _activeCells.Clear();

        foreach (LineOfSightActor actor in _actors)
        {
            actor.Poll(_activeCells, _passiveCells);
        }
    }
}
