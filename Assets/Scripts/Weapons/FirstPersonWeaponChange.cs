using System.Collections.Generic;
using NetworkFramework.NetcodeComponents;
using UnityEngine;

public class FirstPersonWeaponChange : NetworkComponentManager<WeaponNetwork>
{
    private readonly List<GameObject> _weaponModelsRoot = new();
    private GameObject _prevCurrentWeaponModel;

    private Transform _weaponRoot;
    
    private void Awake()
    {
        _weaponRoot = CameraHelper.GetPlayerCamera.GetComponentInChildren<WeaponFirstPersonRoot>().transform;
        foreach (Transform model in _weaponRoot)
        {
            var obj = model.gameObject;
            _weaponModelsRoot.Add(obj);
        }
    }

    protected override void Start()
    {
        base.Start();
        networkComponent.ValueChanged += SetCurrentWeapon;
        SetCurrentWeapon(networkComponent.LocalValue);
    }

    private void OnDestroy()
    {
        networkComponent.ValueChanged -= SetCurrentWeapon;
    }

    private void SetCurrentWeapon(int index)
    {
        if (_prevCurrentWeaponModel != null)
        {
            _prevCurrentWeaponModel.SetActive(false);
        }

        var currentModel = _weaponModelsRoot[index];
        currentModel.SetActive(true);
        _prevCurrentWeaponModel = currentModel;
    }
}