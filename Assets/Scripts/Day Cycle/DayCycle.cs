using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DayCycle {

    public static event System.Action OnHourPassed;
    public static event System.Action OnDayPassed;

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
    public static int Day
    {
        get
        {
            return Mathf.FloorToInt(TotalTime / SecondsInDay) + 1;
        }
    }
    public static int Hour
    {
        get
        {
            return Mathf.FloorToInt(DayPercentage * HOURS_IN_DAY);
        }
    }
    public static int Minute
    {
        get
        {
            return (int)(((DayPercentage * HOURS_IN_DAY) % 1) * MINUTES_IN_HOUR);
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
    
    private const float HOURS_IN_DAY = 24;
    private const float MINUTES_IN_HOUR = 60;
    private const float SECONDS_IN_MINUTE = 60;

    private static float _time;
    private static int _currentHour;
    private static int _currentDay;

    public static void Initialize()
    {
        _time = SecondsInDay * (GameSettings.StartHour / HOURS_IN_DAY);

        _currentDay = Day;
        _currentHour = Hour;
    }
	public static void Update()
    {
        _time += Time.deltaTime * TimeScale;

        if (Hour != _currentHour)
        {
            _currentHour = Hour;

            if (OnHourPassed != null)
                OnHourPassed();
        }

        if (Day != _currentDay)
        {
            _currentDay = Day;

            if (OnDayPassed != null)
                OnDayPassed();
        }
    }
}
