using NetworkFramework.NetcodeComponents;
using UnityEngine;

public class WeaponShot : NetworkComponentManager<PlayerHealthNetwork>
{
    [SerializeField] private CurrentPlayerWeapon currentPlayerWeapon;
    [SerializeField] private ShotManager shotManager;

    private Transform _shotPoint;

    //caching
    private int _damage;
    private int _bulletCount;

    private void Awake()
    {
        _shotPoint = CameraHelper.GetPlayerCamera.transform;
    }

    public void OnWeaponChanged()
    {
        var currentWeaponInfo = currentPlayerWeapon.CurrentWeapon.Weapon.WeaponInfo;
        _bulletCount = currentWeaponInfo.BulletsPerShot <= 1 ? 1 : currentWeaponInfo.BulletsPerShot;
        _damage = currentWeaponInfo.Damage;
    }

    public void OnWeaponShooting()
    {
        shotManager.TakeShot(_shotPoint, _damage, _bulletCount, networkComponent.NetworkObject);
    }
}