using System.Collections;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

public class ShotgunBehavior : SingleShootBehavior
{
    public ShotgunBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent,
        MonoBehaviour coroutineCreateObject) : base(weapon, shootEvent, reloadEvent, coroutineCreateObject)
    {
    }

    protected override IEnumerator ReloadDelayCoroutine()
    {
        yield return new WaitForSeconds(ReloadDelay);
        if (Weapon.CurrentBullets < Weapon.WeaponInfo.ClipSize)
        {
            Weapon.CurrentBullets++;
        }

        Weapon.State = WeaponState.Ready;
        ReloadEvent.Raise(false);
    }
}