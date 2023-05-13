using System.Collections.Generic;
using NetworkFramework.Netcode_Components;
using UnityEngine;

public class WeaponFirstViewer : NetworkComponentManager<PlayerClassNetwork>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private Transform weaponRoot;
    
    private readonly List<GameObject> _weaponModelsRoot = new();
    
    private int _localIndex = -1;
    private GameObject _prevCurrentWeaponModel;

    private void Awake()
    {
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
        SetWeaponModel(networkComponent.GetPlayerClassName());
        SetCurrentWeapon(0);
    }
    
    public void SetCurrentWeapon(int index)
    {
        if (_localIndex == index) return;
        _localIndex = index;
        if (_prevCurrentWeaponModel != null)
        {
            _prevCurrentWeaponModel.SetActive(false);
        }

        var currentModel = _weaponModelsRoot[index];
        currentModel.SetActive(true);
        _prevCurrentWeaponModel = currentModel;
    }

    private void SetWeaponModel(string playerClassName)
    {
        var weaponInfos = playerClassHandler.DataDictionary[playerClassName].weapons;
        for (int i = 0; i < weaponInfos.Count; i++)
        {
            foreach (Transform children in _weaponModelsRoot[i].transform)
            {
                Destroy(children.gameObject);
            }

            //var obj = Instantiate(weaponInfos[i].model, _weaponModelsRoot[i].transform);
            
            //.SetFirstModelSettings(obj);
        }
    }
}