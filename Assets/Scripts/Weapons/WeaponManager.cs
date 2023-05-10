using NetworkFramework.Netcode_Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : NetworkComponentManager<WeaponNetwork>
{
    [SerializeField] private WeaponFirstViewer weaponFirstViewer;
    
    public void OnMainWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            networkComponent.CheckLocalChange(0);
            networkComponent.SetWeaponIndexServerRpc(0);
            weaponFirstViewer.SetCurrentWeapon(0);
        }
    }
    
    public void OnAncillaryWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            networkComponent.CheckLocalChange(1);
            networkComponent.SetWeaponIndexServerRpc(1);
            weaponFirstViewer.SetCurrentWeapon(1);
        }
    }
}