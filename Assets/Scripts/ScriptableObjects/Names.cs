using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Names.asset", menuName = "Game/Names", order = 69)]
public class Names : ScriptableObject {

    public static string GetFirstName()
    {
        return Instance._firstNames.Random();
    }
    public static string GetLastName()
    {
        return Instance._lastNames.Random();
    }
    private static Names Instance { get { return StaticObjects.GetObject<Names>(); } }

    [SerializeField]
    private List<string> _firstNames;
    [SerializeField]
    private List<string> _lastNames;
}
