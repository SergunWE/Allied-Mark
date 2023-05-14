using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.EventSystem.EventParameter;
using Debug = UnityEngine.Debug;

public class SingleShootBehavior : WeaponBehavior
{
    protected readonly Stopwatch ShootStopwatch = new();
    
    public SingleShootBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent) : base(weapon,
        shootEvent, reloadEvent)
    {
        ReloadThread = new Thread(ReloadDelayThread);
        ReloadThread.Start();
        ShootStopwatch.Start();
    }

    public override void ShootBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets <= 0) return;
        var elapsed = ShootStopwatch.Elapsed;
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
        ReloadAutoResetEvent.Set();
        ReloadEvent.Raise(true);
    }

    private void ReloadDelayThread()
    {
        while (true)
        {
            try
            {
                ReloadAutoResetEvent.WaitOne();
                Task.Delay(ReloadDelay - PullDelay, CancellationToken).Wait(CancellationToken);
                Weapon.CurrentBullets = Weapon.WeaponInfo.ClipSize;
                ReloadEvent.Raise(false);
                Task.Delay(PullDelay, CancellationToken).Wait(CancellationToken);
                if (CancellationToken.IsCancellationRequested)
                {
                    continue;
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