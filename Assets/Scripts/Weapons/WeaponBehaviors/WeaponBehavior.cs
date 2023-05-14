using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.EventSystem.EventParameter;
using Debug = UnityEngine.Debug;

public class WeaponBehavior : ICloneable
{
    protected readonly Weapon Weapon;
    protected readonly GameEvent ShootEvent;
    protected readonly GameEventBool ReloadEvent;

    protected TimeSpan ShootDelay;
    protected TimeSpan ReloadDelay;
    protected TimeSpan PullDelay;
    
    protected Thread ReloadThread;
    protected readonly AutoResetEvent ReloadAutoResetEvent = new(false);
    private CancellationTokenSource _cancellationTokenSource = new();
    protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

    protected WeaponBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent)
    {
        Weapon = weapon;
        ShootEvent = shootEvent;
        ReloadEvent = reloadEvent;

        ShootDelay = TimeSpan.FromSeconds(weapon.WeaponInfo.ShootTime);
        ReloadDelay = TimeSpan.FromSeconds(weapon.WeaponInfo.ReloadTime);
        PullDelay = TimeSpan.FromSeconds(weapon.WeaponInfo.PullTime);
        
       
    }

    public virtual async void PullBehavior()
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

    public virtual void ShootBehavior(bool buttonState)
    {
    }

    public virtual void ReloadBehavior(bool buttonState)
    {
    }

    public virtual void StopAllBehaviors()
    {
        ReloadAutoResetEvent.Reset();
        _cancellationTokenSource.Cancel();
        Weapon.State = WeaponState.NotReady;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    ~WeaponBehavior()
    {
        ReloadThread.Abort();
    }
}