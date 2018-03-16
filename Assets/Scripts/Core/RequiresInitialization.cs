using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiresInitialization : MonoBehaviour {

    private void Awake()
    {
        Game.OnHasInitialized += OnInitialized;
        gameObject.SetActive(false);
    }
    private void OnInitialized()
    {
        Game.OnHasInitialized -= OnInitialized;

        gameObject.SetActive(true);
    }
}
