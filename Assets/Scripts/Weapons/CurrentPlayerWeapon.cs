using System;
using System.Collections.Generic;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.NetcodeComponents;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerWeapon : NetworkComponentManager<WeaponNetwork>
{
    [SerializeField] private GameEvent weaponChanged;
    [SerializeField] private GameEvent weaponShooting;
    [SerializeField] private GameEventBool weaponReloading;

    public WeaponBehavior CurrentWeapon { get; private set; }

    private readonly List<WeaponBehavior> _weapons = new();
    private PlayerClassNetwork _playerClassNetwork;

    protected override void Start()
    {
        base.Start();
        try
        {
            var ownerObjects = NetworkManager.Singleton.LocalClient.OwnedObjects;
            foreach (var t in ownerObjects)
            {
                _playerClassNetwork = t.GetComponentInChildren<PlayerClassNetwork>();
                if (_playerClassNetwork != null) break;
            }
        }
        catch
        {
            // ignored
        }

        var playerClass = _playerClassNetwork.PlayerClassInfo;

        foreach (var weaponInfo in playerClass.weapons)
        {
            _weapons.Add(GetWeapon(weaponInfo));
        }

        OnWeaponChanged(networkComponent.LocalValue);
        networkComponent.ValueChanged += OnWeaponChanged;
    }

    private void OnDestroy()
    {
        networkComponent.ValueChanged -= OnWeaponChanged;
    }

    private WeaponBehavior GetWeapon(WeaponInfo weaponInfo)
    {
        var weapon = new Weapon(weaponInfo);

        return weaponInfo.WeaponType switch
        {
            WeaponType.Single => new SingleShootBehavior(weapon, weaponShooting, weaponReloading, this),
            WeaponType.Auto => new AutoShootBehavior(weapon, weaponShooting, weaponReloading, this),
            WeaponType.Shotgun => new ShotgunBehavior(weapon, weaponShooting, weaponReloading, this),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnWeaponChanged(int index)
    {
        CurrentWeapon?.StopAllBehaviors();
        CurrentWeapon = _weapons[index];
        CurrentWeapon.PullBehavior();
        weaponChanged.Raise();
    }
}