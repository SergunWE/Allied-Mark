using System;
using System.Diagnostics;
using NetworkFramework.EventSystem.EventParameter;

public class WeaponBehavior : ICloneable
{
    protected readonly Weapon Weapon;
    protected readonly GameEvent ShootEvent;
    protected readonly GameEventBool ReloadEvent;

    protected readonly Stopwatch ShootStopwatch = new();

    protected TimeSpan ShootDelay;
    protected TimeSpan ReloadDelay;
    protected TimeSpan PullDelay;

    protected WeaponBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent)
    {
        Weapon = weapon;
        ShootEvent = shootEvent;
        ReloadEvent = reloadEvent;

        ShootDelay = TimeSpan.FromSeconds(weapon.WeaponInfo.ShootTime);
        ReloadDelay = TimeSpan.FromSeconds(weapon.WeaponInfo.ReloadTime);
        PullDelay = TimeSpan.FromSeconds(weapon.WeaponInfo.PullTime);
        
        ShootStopwatch.Start();
    }

    public virtual void PullBehavior()
    {
        
    }

    public virtual void ShootBehavior(bool buttonState)
    {
    }

    public virtual void ReloadBehavior(bool buttonState)
    {
    }

    public virtual void StopAllBehaviors()
    {
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}