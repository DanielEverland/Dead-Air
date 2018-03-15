using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needs {

    public Needs()
    {
        DayCycle.OnHourPassed += OnHourPassed;

        _hunger = GameSettings.StartHunger;
        _thirst = GameSettings.StartThirst;
        _rest = GameSettings.StartRest;
    }

    public int Hunger { get { return _hunger; } }
    public int Thirst { get { return _thirst; } }
    public int Rest { get { return _rest; } }

    private int _hunger;
    private int _thirst;
    private int _rest;

    private void OnHourPassed()
    {
        _hunger = Mathf.Clamp(_hunger - GameSettings.HungerDegradation, 0, GameSettings.NEEDS_MAX_VALUE);
        _thirst = Mathf.Clamp(_thirst - GameSettings.ThirstDegradation, 0, GameSettings.NEEDS_MAX_VALUE);
        _rest = Mathf.Clamp(_rest - GameSettings.RestDegradation, 0, GameSettings.NEEDS_MAX_VALUE);
    }
}
