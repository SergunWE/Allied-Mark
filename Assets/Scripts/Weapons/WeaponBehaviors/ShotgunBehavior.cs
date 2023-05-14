using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

public class ShotgunBehavior : SingleShootBehavior
{
    public ShotgunBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent) : base(weapon, shootEvent,
        reloadEvent)
    {
    }
    
    protected override void ReloadDelayThread()
    {
        while (true)
        {
            try
            {
                ReloadAutoResetEvent.WaitOne();
                Task.Delay(ReloadDelay, CancellationToken).Wait(CancellationToken);
                if (CancellationToken.IsCancellationRequested)
                {
                    continue;
                }
                if (Weapon.CurrentBullets < Weapon.WeaponInfo.ClipSize)
                {
                    Weapon.CurrentBullets++;
                }
                Weapon.State = WeaponState.Ready;
                ReloadEvent.Raise(false);
                Debug.Log($@"Reload - {Weapon.CurrentBullets}");
            }
            catch (TaskCanceledException)
            {
            }
            catch (ThreadAbortException)
            {
                return;
            }
        }
        // ReSharper disable once FunctionNeverReturns
    }
}