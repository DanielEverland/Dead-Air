using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KeyManager
{
    public static KeyCode MoveRight { get { return GameSettings.KeyManager._moveRight; } }
    public static KeyCode MoveLeft { get { return GameSettings.KeyManager._moveLeft; } }
    public static KeyCode MoveUp { get { return GameSettings.KeyManager._moveUp; } }
    public static KeyCode MoveDown { get { return GameSettings.KeyManager._moveDown; } }

    public static KeyCode NextColonist { get { return GameSettings.KeyManager._nextColonist; } }
    public static KeyCode PreviousColonist { get { return GameSettings.KeyManager._previousColonist; } }

    public static KeyCode IncreaseTimeScale { get { return GameSettings.KeyManager._increaseTimeScale; } }

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
