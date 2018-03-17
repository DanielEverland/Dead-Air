using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PsychologyManager {

	public static Genders GetRandomGender()
    {
        return (Genders)Random.Range(1, 2);
    }
}
