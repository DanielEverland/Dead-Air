using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSettings : ScriptableObject, ISettings {

    public static float MinutesInDay { get { return Instance._minutesInDay; } }
    public static float DefaultTimeScale { get { return Instance._defaultTimeScale; } }
    public static float IncreasedTimeScale { get { return Instance._increasedTimeScale; } }
    public static float StartHour { get { return Instance._startHour; } }

    private static TimeSettings Instance { get { return GameSettings.GetInstance<TimeSettings>(); } }

    [SerializeField]
    private float _minutesInDay;
    [SerializeField]
    private float _defaultTimeScale;
    [SerializeField]
    private float _increasedTimeScale;
    [SerializeField]
    private float _startHour;
}
