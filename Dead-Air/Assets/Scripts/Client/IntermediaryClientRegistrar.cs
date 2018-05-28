using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntermediaryClientRegistrar {

    private static Dictionary<short, Object> _registeredObjects = new Dictionary<short, Object>();

	public static short Register(Object obj)
    {
        short id = GetRandomID();

        _registeredObjects.Add(id, obj);

        return id;
    }
    public static void Remove(short id)
    {
        _registeredObjects.Remove(id);
    }
    private static short GetRandomID()
    {
        short value = Utility.RandomShort();

        while (_registeredObjects.ContainsKey(value))
        {
            value = Utility.RandomShort();
        }

        return value;
    }
}
