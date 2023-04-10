using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private CurrentWeapon currentWeapon;

    private void Start()
    {
        try
        {
            var ownerObjects = NetworkManager.Singleton.LocalClient.OwnedObjects;
            for (int i = 0; i < ownerObjects.Count; i++)
            {
                currentWeapon = ownerObjects[i].GetComponentInChildren<CurrentWeapon>();
                if (currentWeapon != null) break;
            }
        }
        catch
        {
            // ignored
        }
    }

    public void OnMainWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentWeapon.SetCurrentWeapon(0);
        }
    }
    
    public void OnAncillaryWeaponChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentWeapon.SetCurrentWeapon(1);
        }
    }
}