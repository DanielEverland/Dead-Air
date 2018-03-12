using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private float _speed;

	private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, ColonistManager.SelectedColonist.transform.position, _speed * Time.deltaTime);
    }
}
