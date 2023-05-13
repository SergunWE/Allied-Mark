using System;
using UnityEngine;

[Serializable]
public class SingleShotWeapon : WeaponBehaviorBase
{
    public override void OnShotBehavior(bool state, Transform obj)
    {
        if(!state) return;
        var target = RaycastHelper.GetObject(obj);
        if(target == null) return;
        var component = target.GetComponentInParent<MarkNetwork>();
        if (component == null) return;
        component.DestroyServerRpc();
        // тут стрелять
        Debug.Log("SHOT!!!!!!!!!!!!");
    }

    public override void OnAbilityBehavior(bool state)
    {
    }

    public override void OnReloadBehavior()
    {
    }
}