using NetworkFramework.Netcode_Components;
using UnityEngine.InputSystem;

public class WeaponChange : NetworkComponentManager<WeaponNetwork>
{
    public void OnMainWeaponChanged(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        networkComponent.SetWeaponIndex(0);
    }

    public void OnAncillaryWeaponChanged(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        networkComponent.SetWeaponIndex(1);
    }
}