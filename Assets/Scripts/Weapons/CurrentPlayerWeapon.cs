using System;
using System.Collections.Generic;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.Netcode_Components;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerWeapon : NetworkComponentManager<WeaponNetwork>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    
    [SerializeField] private GameEvent weaponChanged;
    [SerializeField] private GameEvent weaponShooting;
    [SerializeField] private GameEventBool weaponReloading;

    public WeaponBehavior CurrentWeapon { get; private set; }

    private readonly List<WeaponBehavior> _weapons = new();
    private PlayerClassNetwork _playerClassNetwork;

    protected override void Start()
    {
        base.Start();
        networkComponent.ValueChanged += OnWeaponChanged;

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

        string networkClassName = _playerClassNetwork.GetPlayerClassName();
        var playerClass = playerClassHandler.DataDictionary[networkClassName];
        
        foreach (var weaponInfo in playerClass.weapons)
        {
            _weapons.Add(GetWeapon(weaponInfo));
        }

        OnWeaponChanged(networkComponent.LocalValue);
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
            WeaponType.Single => new SingleShootBehavior(weapon, weaponShooting, weaponReloading),
            WeaponType.Auto => throw new ArgumentOutOfRangeException(),
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