using System;

[Serializable]
public class WeaponBehavior : ICloneable
{
    protected Weapon Weapon;
    protected GameEvent ShootEvent;
    protected GameEvent ReloadEvent;

    public WeaponBehavior(Weapon weapon, GameEvent shootEvent, GameEvent reloadEvent)
    {
        Weapon = weapon;
        ShootEvent = shootEvent;
        ReloadEvent = reloadEvent;
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