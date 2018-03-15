using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _zDepth;
    
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        Vector3 newPos = Vector2.Lerp(transform.position, ColonistManager.SelectedColonist.transform.position, _speed * Time.deltaTime);
        newPos.z = _zDepth;

        transform.position = newPos;
    }
}
