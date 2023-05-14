using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

public class WeaponSpeedAnimation : MonoBehaviour
{
    [SerializeField] private CurrentPlayerWeapon currentPlayerWeapon;

    [SerializeField] private GameEventFloat weaponPulling;
    [SerializeField] private GameEventFloat weaponShooting;
    [SerializeField] private GameEventFloat weaponReloading;

    public void OnWeaponChanged()
    {
        var weaponInfo = currentPlayerWeapon.CurrentWeapon.Weapon.WeaponInfo;

        weaponPulling.Raise(1 / weaponInfo.PullTime);
        weaponShooting.Raise(1 / weaponInfo.ShootTime);
        weaponReloading.Raise(1 / weaponInfo.ReloadTime);
    }
}