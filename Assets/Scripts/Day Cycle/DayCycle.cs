using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DayCycle {

    public static float TimeScale
    {
        get
        {
            if (Input.GetKey(KeyManager.IncreaseTimeScale))
            {
                return GameSettings.IncreasedTimeScale;
            }
            else
            {
                return GameSettings.DefaultTimeScale;
            }
        }
    }
    public static float Day
    {
        get
        {
            return Mathf.FloorToInt(TotalTime / SecondsInDay) + 1;
        }
    }
    public static float Hour
    {
        get
        {
            return Mathf.FloorToInt(DayPercentage * HOURS_IN_DAY);
        }
    }
    public static float Minute
    {
        get
        {
            return ((DayPercentage * HOURS_IN_DAY) % 1) * MINUTES_IN_HOUR;
        }
    }
    public static float DayPercentage
    {
        get
        {
            return (TotalTime % SecondsInDay) / SecondsInDay;
        }
    }
    public static float SecondsInDay
    {
        get
        {
            return GameSettings.MinutesInDay * SECONDS_IN_MINUTE;
        }
    }
    public static float TotalTime
    {
        get
        {
            return _time;
        }
    }

    private static float _time;

    private const float HOURS_IN_DAY = 24;
    private const float MINUTES_IN_HOUR = 60;
    private const float SECONDS_IN_MINUTE = 60;

	public static void Update()
    {
        _time += Time.deltaTime * TimeScale;
    }
}
