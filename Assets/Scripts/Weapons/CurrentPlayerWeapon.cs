using System.Collections.Generic;
using NetworkFramework.Netcode_Components;
using Unity.Netcode;
using UnityEngine;

public class CurrentPlayerWeapon : NetworkComponentManager<WeaponNetwork>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private GameEvent weaponChanged;
    
    public Weapon CurrentWeapon { get; private set; }

    private readonly List<Weapon> _weapons = new();
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
            _weapons.Add(new Weapon(weaponInfo));
        }

        OnWeaponChanged(networkComponent.LocalValue);
    }

    private void OnDestroy()
    {
        networkComponent.ValueChanged -= OnWeaponChanged;
    }

    private void OnWeaponChanged(int index)
    {
        CurrentWeapon = _weapons[index];
        weaponChanged.Raise();
    }
}