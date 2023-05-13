using System;
using UnityEngine;

[Serializable]
public abstract class WeaponBehaviorBase
{
    public abstract void OnShotBehavior(bool state, Transform obj);
    public abstract void OnAbilityBehavior(bool state);
    public abstract void OnReloadBehavior();
}