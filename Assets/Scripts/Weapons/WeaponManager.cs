using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CurrentPlayerWeapon))]
public class WeaponManager : MonoBehaviour
{
    private CurrentPlayerWeapon _currentPlayerWeapon;

    private void Awake()
    {
        _currentPlayerWeapon = GetComponent<CurrentPlayerWeapon>();
    }

    public void OnShotButtonClicked(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        if (_currentPlayerWeapon.CurrentWeapon.CurrentBullets <= 0) return;
        _currentPlayerWeapon.CurrentWeapon.CurrentBullets--;
        Debug.Log($@"Shoot - { _currentPlayerWeapon.CurrentWeapon.CurrentBullets}");
    }
    
    public void OnReloadButtonClicked(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        _currentPlayerWeapon.CurrentWeapon.CurrentBullets = _currentPlayerWeapon.CurrentWeapon.WeaponInfo.ClipSize;
        Debug.Log($@"Reload - { _currentPlayerWeapon.CurrentWeapon.CurrentBullets}");
    }
}