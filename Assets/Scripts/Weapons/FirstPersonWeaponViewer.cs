using System.Collections.Generic;
using NetworkFramework.NetcodeComponents;
using Unity.Collections;
using UnityEngine;

public class FirstPersonWeaponViewer : NetworkComponentManager<PlayerClassNetwork>
{
    [SerializeField] private Transform weaponRoot;
    [SerializeField] private PlayerClassHandler playerClassHandler;

    private readonly List<GameObject> _weaponModelsRoot = new();

    protected void Awake()
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
        if(string.IsNullOrEmpty(playerClassName.Value)) return;
        var weaponInfos = playerClassHandler.DataDictionary[playerClassName.Value].weapons;
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