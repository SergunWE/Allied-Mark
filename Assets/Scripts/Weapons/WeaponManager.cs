using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CurrentPlayerWeapon))]
public class WeaponManager : MonoBehaviour
{
    private CurrentPlayerWeapon _currentPlayerWeapon;
    private WeaponBehavior CurrentWeapon => _currentPlayerWeapon.CurrentWeapon;

    private void Awake()
    {
        _currentPlayerWeapon = GetComponent<CurrentPlayerWeapon>();
    }

    public void OnShotButtonClicked(InputAction.CallbackContext context)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (context.phase)
        {
            case InputActionPhase.Started:
                CurrentWeapon.ShootBehavior(true);
                break;
            case InputActionPhase.Canceled:
                CurrentWeapon.ShootBehavior(false);
                break;
        }
    }
    
    public void OnReloadButtonClicked(InputAction.CallbackContext context)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (context.phase)
        {
            case InputActionPhase.Started:
                CurrentWeapon.ReloadBehavior(true);
                break;
            case InputActionPhase.Canceled:
                CurrentWeapon.ReloadBehavior(false);
                break;
        }
    }
}