using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightActor : MonoBehaviour {

    public float Radius { get { return _radius; } }

    [SerializeField]
    private float _radius = 5;
}
