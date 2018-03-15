using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LineOfSightHider : MonoBehaviour {

    [SerializeField]
    private Renderer _renderer;

    private void Start()
    {
        LineOfSightManager.OnUpdate += Poll;
    }
    private void OnDestroy()
    {
        LineOfSightManager.OnUpdate -= Poll;
    }
    private void OnValidate()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
    }
    private void Poll()
    {
        Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

        _renderer.enabled = LineOfSightManager.GetState(pos) == LineOfSightState.Active;
    }
}
