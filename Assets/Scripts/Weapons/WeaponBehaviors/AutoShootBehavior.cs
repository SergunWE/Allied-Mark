using System.Collections;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;


public class AutoShootBehavior : SingleShootBehavior
{
    private Coroutine _shootCoroutine;

    public AutoShootBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent, 
        MonoBehaviour coroutineCreateObject) : base(weapon, shootEvent, reloadEvent, coroutineCreateObject)
    {
    }

    public override void ShootBehavior(bool buttonState)
    {
        if (buttonState)
        {
            _shootCoroutine = CoroutineCreateObject.StartCoroutine(ShootLoopCoroutine());
        }
        else
        {
            if (_shootCoroutine != null)
            {
                CoroutineCreateObject.StopCoroutine(_shootCoroutine);
            }
        }
    }

    private IEnumerator ShootLoopCoroutine()
    {
        while (true)
        {
            if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets <= 0) break;
            var elapsed = ShootStopwatch.Elapsed.TotalSeconds;
            if (elapsed <= ShootDelay) continue;
            Weapon.CurrentBullets--;
            ShootEvent.Raise();
            ShootStopwatch.Restart();
            yield return new WaitForSeconds(ShootDelay);
        }
        yield return null;
    }

   
}