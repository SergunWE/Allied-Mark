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

    public void OnWeaponChanged(InputAction.CallbackContext context)
    {
        currentWeapon.weaponName.Value = "test";
    }
}