using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugDataText : MonoBehaviour {

    private readonly List<System.Func<string>> _dataFunctions = new List<System.Func<string>>()
    {
        MousePosition,

        Space,

        Time,

        Space,

        ColonistData,
    };

    [SerializeField]
    private Text _text;
    
    private static string ColonistData()
    {
        return string.Format("{0}\nHunger: {1} ({2}%)\nThirst: {3} ({4}%)\nRest: {5} ({6}%)",
            ColonistManager.SelectedColonist.FullName,
            ColonistManager.SelectedColonist.Needs.Hunger,
            ((float)ColonistManager.SelectedColonist.Needs.Hunger / (float)PsychologySettings.NEEDS_MAX_VALUE) * 100,
            ColonistManager.SelectedColonist.Needs.Thirst,
            ((float)ColonistManager.SelectedColonist.Needs.Thirst / (float)PsychologySettings.NEEDS_MAX_VALUE) * 100,
            ColonistManager.SelectedColonist.Needs.Rest,
            ((float)ColonistManager.SelectedColonist.Needs.Rest / (float)PsychologySettings.NEEDS_MAX_VALUE) * 100);
    }
    private static string Time()
    {
        return string.Format("{0}:{1}\nDay {2} ({3}%)\n{4} ({5})",
            DayCycle.Hour.ToString("00"),
            DayCycle.Minute.Floor(5).ToString("00"),
            DayCycle.Day,
            (DayCycle.DayPercentage * 100).ToString("F0"),
            DayCycle.TotalTime.ToString("F1"),
            DayCycle.TimeScale);
    }
    private static string Space()
    {
        return "\n";
    }
    private static string MousePosition()
    {
        Vector2 screenSpace = Input.mousePosition;
        Vector2 worldSpace = Camera.main.ScreenToWorldPoint(screenSpace);
        Vector2 tilePosition = Utility.WorldToTilePos(worldSpace);

        return string.Format("Tile: {0}\nWorld: {1}\nScreen: {2}", tilePosition.ToString("F0"), worldSpace.ToString("F0"), screenSpace.ToString("F0"));
    }
    private void Update()
    {
        string output = "";

        for (int i = 0; i < _dataFunctions.Count; i++)
        {
            string functionOutput = _dataFunctions[i]();

            if (functionOutput != "" && functionOutput != string.Empty && functionOutput != default(string))
                output += functionOutput += "\n";
        }

        _text.text = output;
    }
}
