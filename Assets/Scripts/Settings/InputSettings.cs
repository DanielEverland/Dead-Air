using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSettings : ScriptableObject, ISettings {

	public static KeyCode MoveRight { get { return Instance._moveRight; } }
    public static KeyCode MoveLeft { get { return Instance._moveLeft; } }
    public static KeyCode MoveUp { get { return Instance._moveUp; } }
    public static KeyCode MoveDown { get { return Instance._moveDown; } }

    public static KeyCode NextColonist { get { return Instance._nextColonist; } }
    public static KeyCode PreviousColonist { get { return Instance._previousColonist; } }

    public static KeyCode IncreaseTimeScale { get { return Instance._increaseTimeScale; } }

    private static InputSettings Instance { get { return GameSettings.GetInstance<InputSettings>(); } }

    [Header("Movement")]
    [SerializeField]
    private KeyCode _moveRight;
    [SerializeField]
    private KeyCode _moveLeft;
    [SerializeField]
    private KeyCode _moveUp;
    [SerializeField]
    private KeyCode _moveDown;


    [Header("Colonists")]
    [SerializeField]
    private KeyCode _nextColonist;
    [SerializeField]
    private KeyCode _previousColonist;

    [Header("Commands")]
    [SerializeField]
    private KeyCode _increaseTimeScale;
}
