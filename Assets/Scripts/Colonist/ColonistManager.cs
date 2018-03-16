using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColonistManager {

    public static Colonist SelectedColonist { get { return _colonists[ColonistIndex]; } }

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

        for (int i = 0; i < GeneralSettings.InitialColonists; i++)
        {
            CreateColonist();
        }
    }
    public static void CreateColonist()
    {
        Colonist newColonist = GameObject.Instantiate(Mods.GetObject<Colonist>("Colonist"));
        newColonist.name = string.Format("Colonist - {0}", newColonist.FullName);

        SceneOrganizer.Add("Colonists", newColonist.gameObject);

        _colonists.Add(newColonist);
    }
    public static void OnUpdate()
    {
        PollIndex();

        SelectedColonist.Poll();
    }
    private static void PollIndex()
    {
        if(Input.GetKeyDown(InputSettings.NextColonist))
        {
            ColonistIndex++;
        }
        else if(Input.GetKeyDown(InputSettings.PreviousColonist))
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
