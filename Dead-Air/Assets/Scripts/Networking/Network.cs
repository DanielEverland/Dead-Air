using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour {

    public bool IsServer { get { return Server.IsInitialized; } }
    public bool IsClient { get { return !IsServer; } }

    private static Network _instance;

    private static List<System.Action> _updateDelegates = new List<System.Action>();

    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        for (int i = 0; i < _updateDelegates.Count; i++)
            _updateDelegates[i].Invoke();
    }
    public static void RegisterUpdateHandler(System.Action callback)
    {
        _updateDelegates.Add(callback);
    }
    public static void UnregisterUpdateHandler(System.Action callback)
    {
        _updateDelegates.Remove(callback);
    }
}
