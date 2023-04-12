using UnityEngine.InputSystem;

public class WeaponManager : NetworkComponentManager<WeaponNetwork>
{
    public void OnMainWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            networkComponent.SetCurrentWeaponServerRpc(0);
        }
    }
    
    public void OnAncillaryWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            networkComponent.SetCurrentWeaponServerRpc(1);
        }
    }
}