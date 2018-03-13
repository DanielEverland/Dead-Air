using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingGenerator {
    
    public static void Initialize()
    {
        Place(new SafeHouse());
    }
    public static void Place(Building building)
    {
        if (building.StaticPosition)
        {
            building.Place();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
