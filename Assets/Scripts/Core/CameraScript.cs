using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _zDepth;
    [SerializeField]
    private GameObject _light;
    
    private void Update()
    {
        if (!Game.HasInitialized)
            return;

        Move();
        SetLight();
    }
    private void SetLight()
    {
        _light.transform.position = ColonistManager.SelectedColonist.transform.position + new Vector3(0, 0, -1);
    }
    private void Move()
    {
        Vector3 newPos = Vector2.Lerp(transform.position, ColonistManager.SelectedColonist.transform.position, _speed * Time.deltaTime);
        newPos.z = _zDepth;

        transform.position = newPos;
    }
}
