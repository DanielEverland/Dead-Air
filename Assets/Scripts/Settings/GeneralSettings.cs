using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSettings : ScriptableObject, ISettings {

    public static float InitialColonists { get { return Instance._initialColonists; } }

    private static GeneralSettings Instance { get { return GameSettings.GetInstance<GeneralSettings>(); } }

    [SerializeField]
    private int _initialColonists;
}
