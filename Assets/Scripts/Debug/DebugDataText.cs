using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugDataText : MonoBehaviour {

    private readonly List<System.Func<string>> _dataFunctions = new List<System.Func<string>>()
    {
        MousePosition,
    };

    [SerializeField]
    private Text _text;
    
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
            string functionOutput = _dataFunctions[0]();

            if (functionOutput != "" && functionOutput != string.Empty && functionOutput != default(string))
                output += functionOutput += "\n";
        }

        _text.text = output;
    }
}
