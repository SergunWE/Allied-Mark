using UnityEngine;

[System.Serializable]
public class SingleShootBehavior : WeaponBehavior
{
    public SingleShootBehavior(Weapon weapon, GameEvent shootEvent, GameEvent reloadEvent) : base(weapon, shootEvent,
        reloadEvent)
    {
    }

    public override void ShootBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets <= 0) return;
        Weapon.CurrentBullets--;
        Debug.Log($@"Shoot - { Weapon.CurrentBullets}");
        ShootEvent.Raise();
    }

    public override void ReloadBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready) return;
        Weapon.CurrentBullets = Weapon.WeaponInfo.ClipSize;
        Debug.Log($@"Reload - { Weapon.CurrentBullets}");
        ReloadEvent.Raise();
    }

    public override void StopAllBehaviors()
    {
        
    }
}