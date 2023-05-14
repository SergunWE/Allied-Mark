using System;
using System.Collections;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

public class WeaponBehavior : ICloneable
{
    public Weapon Weapon { get; }
    protected readonly GameEvent ShootEvent;
    protected readonly GameEventBool ReloadEvent;
    protected readonly MonoBehaviour CoroutineCreateObject;

    protected float ShootDelay;
    protected float ReloadDelay;
    protected float PullDelay;

    protected Coroutine ReloadCoroutine;
    private Coroutine _pullingCoroutine;

    protected WeaponBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent,
        MonoBehaviour coroutineCreateObject)
    {
        Weapon = weapon;
        ShootEvent = shootEvent;
        ReloadEvent = reloadEvent;
        CoroutineCreateObject = coroutineCreateObject;

        ShootDelay = weapon.WeaponInfo.ShootTime;
        ReloadDelay = weapon.WeaponInfo.ReloadTime;
        PullDelay = weapon.WeaponInfo.PullTime;
    }

    public virtual void PullBehavior()
    {
        _pullingCoroutine = CoroutineCreateObject.StartCoroutine(PullingDelayCoroutine());
    }

    public virtual void ShootBehavior(bool buttonState)
    {
    }

    public virtual void ReloadBehavior(bool buttonState)
    {
    }

    public virtual void StopAllBehaviors()
    {
        Weapon.State = WeaponState.NotReady;
        CoroutineCreateObject.StopAllCoroutines();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    private IEnumerator PullingDelayCoroutine()
    {
        Weapon.State = WeaponState.NotReady;
        yield return new WaitForSeconds(PullDelay);
        Weapon.State = WeaponState.Ready;
    }

    ~WeaponBehavior()
    {
        CoroutineCreateObject.StopAllCoroutines();
    }
}