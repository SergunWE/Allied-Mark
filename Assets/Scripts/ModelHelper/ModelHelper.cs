using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public static class ModelHelper
{
    public static void SetOwnerModelSettings(NetworkBehaviour network, GameObject obj)
    {
        if (!network.IsOwner) return;

        var smrComps =  FindComponents<SkinnedMeshRenderer>(obj);
        foreach (var comp in smrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
       
        var mrComps =  FindComponents<MeshRenderer>(obj);
        foreach (var comp in mrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
    }
    
    public static void SetFirstModelSettings(GameObject obj)
    {
        var smrComps =  FindComponents<SkinnedMeshRenderer>(obj);
        foreach (var comp in smrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
       
        var mrComps =  FindComponents<MeshRenderer>(obj);
        foreach (var comp in mrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.Off;
        }
    }

    public static List<T> FindComponents<T>(GameObject gameObject)
    {
        var comps = new List<T>();
        comps.AddRange(gameObject.GetComponents<T>());
        comps.AddRange(gameObject.GetComponentsInChildren<T>());
        return comps;
    }
}