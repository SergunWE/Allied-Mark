using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;


public class AutoShootBehavior : SingleShootBehavior
{
    private readonly Thread _shootThread;
    private readonly AutoResetEvent _shootAutoResetEvent = new(false);
    private bool _shootButtonPressed;
    
    public AutoShootBehavior(Weapon weapon, GameEvent shootEvent, GameEventBool reloadEvent) : base(weapon, shootEvent,
        reloadEvent)
    {
        _shootThread = new Thread(ShootThread);
        _shootThread.Start();
    }

    public override void ShootBehavior(bool buttonState)
    {
        _shootButtonPressed = buttonState;
        if (buttonState)
        {
            _shootAutoResetEvent.Set();
        }
    }

    public override void StopAllBehaviors()
    {
        base.StopAllBehaviors();
        _shootButtonPressed = false;
        _shootAutoResetEvent.Reset();
    }

    private void ShootThread()
    {
        while (true)
        {
            try
            {
                _shootAutoResetEvent.WaitOne();
                while (_shootButtonPressed)
                {
                    if (Weapon.State != WeaponState.Ready || Weapon.CurrentBullets <= 0) break;
                    var elapsed = ShootStopwatch.Elapsed;
                    if (elapsed <= ShootDelay) continue;
                    Weapon.CurrentBullets--;
                    Debug.Log($@"Shoot - {Weapon.CurrentBullets}");
                    ShootEvent.Raise();
                    ShootStopwatch.Restart();
                    Task.Delay(ShootDelay).Wait();
                }
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

    ~AutoShootBehavior()
    {
        _shootThread.Abort();
    }
}