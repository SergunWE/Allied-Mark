using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInput : MonoBehaviour
{
    [SerializeField] private CurrentPlayerWeapon currentPlayerWeapon;
    private WeaponBehavior _currentWeapon;
    
    public void OnWeaponChanged()
    {
        _currentWeapon = currentPlayerWeapon.CurrentWeapon;
    }

    public void OnShotButtonClicked(InputAction.CallbackContext context)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _currentWeapon.ShootBehavior(true);
                break;
            case InputActionPhase.Canceled:
                _currentWeapon.ShootBehavior(false);
                break;
        }
    }
    
    public void OnReloadButtonClicked(InputAction.CallbackContext context)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _currentWeapon.ReloadBehavior(true);
                break;
            case InputActionPhase.Canceled:
                _currentWeapon.ReloadBehavior(false);
                break;
        }
    }
}