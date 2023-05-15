using System.Collections.Generic;
using UnityEngine;

public static class ComponentHelper
{
    public static List<T> FindComponents<T>(GameObject gameObject)
    {
        var comps = new List<T>();
        comps.AddRange(gameObject.GetComponents<T>());
        comps.AddRange(gameObject.GetComponentsInChildren<T>());
        return comps;
    }

    public static T FindComponent<T>(GameObject gameObject)
    {
        if (!gameObject.TryGetComponent<T>(out var comp))
        {
            comp = gameObject.GetComponentInChildren<T>();
        }

        return comp;
    }
}