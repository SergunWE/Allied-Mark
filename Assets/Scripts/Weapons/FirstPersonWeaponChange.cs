using System.Collections.Generic;
using NetworkFramework.Netcode_Components;
using UnityEngine;

public class FirstPersonWeaponChange : NetworkComponentManager<WeaponNetwork>
{
    [SerializeField] private Transform weaponRoot;

    private readonly List<GameObject> _weaponModelsRoot = new();
    private GameObject _prevCurrentWeaponModel;

    private void Awake()
    {
        if (weaponRoot == null)
        {
            if (Camera.main != null)
            {
                weaponRoot = Camera.main.transform.GetChild(0);
            }
        }
        foreach (Transform model in weaponRoot)
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