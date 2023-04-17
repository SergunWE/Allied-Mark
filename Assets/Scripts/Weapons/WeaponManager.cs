using NetworkFramework.Netcode_Components;
using UnityEngine.InputSystem;

public class WeaponManager : NetworkComponentManager<WeaponNetwork>
{
    public void OnMainWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            networkComponent.CheckLocalChange(1);
            networkComponent.SetWeaponIndexServerRpc(0);
            
        }
    }
    
    public void OnAncillaryWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            networkComponent.CheckLocalChange(1);
            networkComponent.SetWeaponIndexServerRpc(1);
        }
    }
}