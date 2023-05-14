using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

public class SingleShootBehavior : WeaponBehavior
{
    private readonly Thread _reloadThread;
    private readonly AutoResetEvent _reloadAutoResetEvent = new(false);
    private CancellationTokenSource _cancellationTokenSource = new();
    private CancellationToken CancellationToken => _cancellationTokenSource.Token;

    public SingleShootBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent) : base(weapon,
        shootEvent, reloadEvent)
    {
        _reloadThread = new Thread(ReloadDelayThread);
        _reloadThread.Start();
    }

    public override async void PullBehavior()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        Debug.Log("Weapon pulling");
        Weapon.State = WeaponState.NotReady;
        try
        {
            await Task.Delay(PullDelay, CancellationToken);
            Weapon.State = WeaponState.Ready;
            Debug.Log("Weapon pulling done");
        }
        catch (TaskCanceledException)
        {
        }
    }

    public override void ShootBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets <= 0) return;
        var elapsed = ShootStopwatch.Elapsed;
        if (elapsed <= ShootDelay) return;
        Weapon.CurrentBullets--;
        Debug.Log($@"Shoot - {Weapon.CurrentBullets}");
        ShootEvent.Raise();
    }

    public override void ReloadBehavior(bool buttonState)
    {
        if (!buttonState) return;
        if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets >= Weapon.WeaponInfo.ClipSize) return;
        Weapon.State = WeaponState.NotReady;
        _reloadAutoResetEvent.Set();
        ReloadEvent.Raise(true);
    }

    public override void StopAllBehaviors()
    {
        _reloadAutoResetEvent.Reset();
        _cancellationTokenSource.Cancel();
        Weapon.State = WeaponState.NotReady;
    }

    private void ReloadDelayThread()
    {
        while (true)
        {
            try
            {
                _reloadAutoResetEvent.WaitOne();
                Task.Delay(ReloadDelay, CancellationToken).Wait(CancellationToken);
                if (CancellationToken.IsCancellationRequested)
                {
                    continue;
                }

                Weapon.CurrentBullets = Weapon.WeaponInfo.ClipSize;
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

    ~SingleShootBehavior()
    {
        _reloadThread.Abort();
    }
}