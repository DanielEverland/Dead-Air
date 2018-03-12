using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColonistManager {

    private static int ColonistIndex
    {
        get
        {
            return Wrap(_colonistIndex, 0, _colonists.Count - 1);
        }
        set
        {
            _colonistIndex = Wrap(value, 0, _colonists.Count - 1);
        }
    }

    private static List<Colonist> _colonists;
    private static int _colonistIndex;

	public static void Initialize()
    {
        _colonists = new List<Colonist>();

        for (int i = 0; i < GameSettings.InitialColonists; i++)
        {
            CreateColonist();
        }
    }
    public static void CreateColonist()
    {
        Colonist newColonist = GameObject.Instantiate(StaticObjects.GetObject<Colonist>());
        _colonists.Add(newColonist);
    }
    public static void OnUpdate()
    {
        PollIndex();

        _colonists[ColonistIndex].Poll();
    }
    private static void PollIndex()
    {
        if(Input.GetKeyDown(KeyManager.NextColonist))
        {
            ColonistIndex++;
        }
        else if(Input.GetKeyDown(KeyManager.PreviousColonist))
        {
            ColonistIndex--;
        }
    }
    private static int Wrap(int value, int min, int max)
    {
        if (value < min)
        {
            value = max;
        }
        else if (value > max)
        {
            value = min;
        }

        return value;
    }
}
