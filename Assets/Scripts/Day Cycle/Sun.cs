using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Sun : MonoBehaviour {

    [SerializeField]
    private Light _light;
    [SerializeField]
    private Gradient _ambientColor;

    private void OnValidate()
    {
        _light = GetComponent<Light>();
    }
    private void Update()
    {
        AnimateColor();
    }
    private void AnimateColor()
    {
        RenderSettings.ambientLight = _ambientColor.Evaluate(DayCycle.DayPercentage);
    }
}
