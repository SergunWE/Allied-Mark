using System.Collections.Generic;
using NetworkFramework.NetcodeComponents;
using Unity.Collections;
using UnityEngine;

public class FirstPersonWeaponViewer : NetworkComponentManager<PlayerClassNetwork>
{
    private Transform _weaponRoot;
    private readonly List<GameObject> _weaponModelsRoot = new();

    protected void Awake()
    {
        _weaponRoot = CameraHelper.GetPlayerCamera.GetComponentInChildren<WeaponFirstPersonRoot>().transform;
        foreach (Transform model in _weaponRoot)
        {
            var obj = model.gameObject;
            _weaponModelsRoot.Add(obj);
            obj.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
        networkComponent.ValueChanged += SetWeaponModel;
        SetWeaponModel(networkComponent.LocalValue.Value);
    }

    private void OnDestroy()
    {
        networkComponent.ValueChanged -= SetWeaponModel;
    }

    private void SetWeaponModel(FixedString128Bytes playerClassName)
    {
        if(playerClassName.IsEmpty) return;
        var weaponInfos = networkComponent.PlayerClassInfo.weapons;
        for (int i = 0; i < weaponInfos.Count; i++)
        {
            foreach (Transform children in _weaponModelsRoot[i].transform)
            {
                Destroy(children.gameObject);
            }

            Instantiate(weaponInfos[i].FirstPersonModel, _weaponModelsRoot[i].transform);
        }
    }
}