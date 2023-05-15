using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public static class ModelHelper
{
    public static void SetOwnerModelSettings(NetworkBehaviour network, GameObject obj)
    {
        if (!network.IsOwner) return;

        var smrComps = ComponentHelper.FindComponents<SkinnedMeshRenderer>(obj);
        foreach (var comp in smrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

        var mrComps = ComponentHelper.FindComponents<MeshRenderer>(obj);
        foreach (var comp in mrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
    }

    public static void SetFirstModelSettings(GameObject obj)
    {
        var smrComps = ComponentHelper.FindComponents<SkinnedMeshRenderer>(obj);
        foreach (var comp in smrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

        var mrComps = ComponentHelper.FindComponents<MeshRenderer>(obj);
        foreach (var comp in mrComps)
        {
            comp.shadowCastingMode = ShadowCastingMode.Off;
        }
    }
}