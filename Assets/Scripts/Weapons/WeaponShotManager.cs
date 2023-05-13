using System.Collections.Generic;
using NetworkFramework.Netcode_Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using WeaponSystem;

public class WeaponShotManager : NetworkComponentManager<PlayerClassNetwork>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [FormerlySerializedAs("weaponChangeManager")] [FormerlySerializedAs("weaponManager")] [SerializeField] private WeaponChange weaponChange;
    
    [SerializeField] private Camera playerCamera;

    private List<WeaponInfo> _weaponInfos;
    private List<CurrentWeaponAmmunition> _weaponAmmunitions = new();

    protected void Start()
    {
        string networkClassName = networkComponent.GetPlayerClassName();
        _weaponInfos = playerClassHandler.DataDictionary[networkClassName].weapons;
        foreach (var info in _weaponInfos)
        {
            _weaponAmmunitions.Add(new CurrentWeaponAmmunition()
            {
               // CurrentAmmo = info.clipSize,
                //AmmoSize = info.clipSize
            });
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        // if (context.started)
        // {
        //     if (_weaponAmmunitions[weaponManager.Index].CurrentAmmo > 0)
        //     {
        //         //_weaponInfos[weaponManager.Index].weaponBehaviorBase.OnShotBehavior(true, playerCamera.transform);
        //     }
        // }
        // else
        // {
        //     if (context.canceled)
        //     {
        //         if (_weaponAmmunitions[weaponManager.Index].CurrentAmmo > 0)
        //         {
        //             //_weaponInfos[weaponManager.Index].weaponBehaviorBase.OnShotBehavior(false, playerCamera.transform);
        //         }
        //     }
        // }
    }
}