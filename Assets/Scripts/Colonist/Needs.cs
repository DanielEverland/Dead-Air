using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needs {

    public Needs()
    {
        DayCycle.OnHourPassed += OnHourPassed;

        _hunger = PsychologySettings.StartHunger;
        _thirst = PsychologySettings.StartThirst;
        _rest = PsychologySettings.StartRest;
    }

    public int Hunger { get { return _hunger; } }
    public int Thirst { get { return _thirst; } }
    public int Rest { get { return _rest; } }

    private int _hunger;
    private int _thirst;
    private int _rest;

    private void OnHourPassed()
    {
        _hunger = Mathf.Clamp(_hunger - PsychologySettings.HungerDegradation, 0, PsychologySettings.NEEDS_MAX_VALUE);
        _thirst = Mathf.Clamp(_thirst - PsychologySettings.ThirstDegradation, 0, PsychologySettings.NEEDS_MAX_VALUE);
        _rest = Mathf.Clamp(_rest - PsychologySettings.RestDegradation, 0, PsychologySettings.NEEDS_MAX_VALUE);
    }
}
