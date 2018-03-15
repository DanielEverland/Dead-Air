using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineOfSightManager {

    public static event System.Action OnUpdate;

    private static List<LineOfSightActor> _actors;

    /// <summary>
    /// How many times a second should we update the texture
    /// </summary>
    private const float REFRESH_RATE = 10;

    private static float _timeSinceLastUpdate;
    private static HashSet<Vector2Int> _passive;
    private static HashSet<Vector2Int> _current;
    private static HashSet<Vector2Int> _previous;

    public static void Initialize()
    {
        _actors = new List<LineOfSightActor>();
        _previous = new HashSet<Vector2Int>();
        _current = new HashSet<Vector2Int>();
        _passive = new HashSet<Vector2Int>();
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
        if (_current.Contains(worldPosition))
        {
            return LineOfSightState.Active;
        }
        else if (_passive.Contains(worldPosition))
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

            _previous = _current;

            Poll();
            CallRenderer();

            if (OnUpdate != null)
                OnUpdate();
        }
    }
    private static void Poll()
    {
        _current = new HashSet<Vector2Int>();

        foreach (LineOfSightActor actor in _actors)
        {
            actor.Poll(_current);
        }
    }
    private static void CallRenderer()
    {
        List<Vector2Int> requireUpdate = new List<Vector2Int>();

        foreach (Vector2Int pos in _current.Union(_previous))
        {
            if (!_passive.Contains(pos))
                _passive.Add(pos);

            if (!_previous.Contains(pos) || !_current.Contains(pos))
                requireUpdate.Add(pos);
        }

        LineOfSightRenderer.Render(requireUpdate);
    }
}
