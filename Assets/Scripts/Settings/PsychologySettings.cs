using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychologySettings : ScriptableObject, ISettings {

    public const int NEEDS_MAX_VALUE = 1000;

    public static int StartHunger { get { return Instance._startHunger; } }
    public static int StartThirst { get { return Instance._startThirst; } }
    public static int StartRest { get { return Instance._startRest; } }
    public static int HungerDegradation { get { return Instance._hungerDegradation; } }
    public static int ThirstDegradation { get { return Instance._thirstDegradation; } }
    public static int RestDegradation { get { return Instance._restDegradation; } }

    private static PsychologySettings Instance { get { return GameSettings.GetInstance<PsychologySettings>(); } }

    [Range(0, NEEDS_MAX_VALUE)]
    [SerializeField]
    private int _startHunger;
    [Range(0, NEEDS_MAX_VALUE)]
    [SerializeField]
    private int _startThirst;
    [Range(0, NEEDS_MAX_VALUE)]
    [SerializeField]
    private int _startRest;

    [Space()]
    [SerializeField]
    private int _hungerDegradation;
    [SerializeField]
    private int _thirstDegradation;
    [SerializeField]
    private int _restDegradation;
}
