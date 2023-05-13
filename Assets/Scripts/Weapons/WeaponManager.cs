using NetworkFramework.Netcode_Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : NetworkComponentManager<WeaponNetwork>
{
    [SerializeField] private WeaponFirstViewer weaponFirstViewer;

    public void OnMainWeaponChanged(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        networkComponent.SetWeaponIndex(0);
        weaponFirstViewer.SetCurrentWeapon(0);
    }

    public void OnAncillaryWeaponChanged(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        networkComponent.SetWeaponIndex(1);
        weaponFirstViewer.SetCurrentWeapon(1);
    }
}