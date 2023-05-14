using System.Collections;
using System.Diagnostics;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SingleShootBehavior : WeaponBehavior
{
    protected readonly Stopwatch ShootStopwatch = new();

    public SingleShootBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent,
        MonoBehaviour coroutineCreateObject) : base(weapon, shootEvent, reloadEvent, coroutineCreateObject)
    {
        ShootStopwatch.Start();
    }

    public override void ShootBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets <= 0) return;
        var elapsed = ShootStopwatch.Elapsed.TotalSeconds;
        if (elapsed <= ShootDelay) return;
        Weapon.CurrentBullets--;
        Debug.Log($@"Shoot - {Weapon.CurrentBullets}");
        ShootStopwatch.Restart();
        ShootEvent.Raise();
    }

    public override void ReloadBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets >= Weapon.WeaponInfo.ClipSize) return;
        Weapon.State = WeaponState.NotReady;
        ReloadCoroutine = CoroutineCreateObject.StartCoroutine(ReloadDelayCoroutine());
        ReloadEvent.Raise(true);
    }

    protected virtual IEnumerator ReloadDelayCoroutine()
    {
        yield return new WaitForSeconds(ReloadDelay - PullDelay);
        Weapon.CurrentBullets = Weapon.WeaponInfo.ClipSize;
        ReloadEvent.Raise(false);
        yield return new WaitForSeconds(PullDelay);
        Weapon.State = WeaponState.Ready;
        ReloadEvent.Raise(false);
    }
    // ReSharper disable once FunctionNeverReturns

}