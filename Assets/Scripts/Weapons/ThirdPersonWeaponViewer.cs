using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ThirdPersonWeaponViewer : MonoBehaviour
{
    [SerializeField] private WeaponNetwork weaponNetwork;
    [SerializeField] private PlayerClassNetwork playerClassNetwork;
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private Transform weaponRoot;
    
    private readonly List<GameObject> _weaponModelsRoot = new();
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

    private void Start()
    {
        SetCurrentWeapon(weaponNetwork.LocalValue);
    }

    private void OnEnable()
    {
        weaponNetwork.ValueChanged += SetCurrentWeapon;
        playerClassNetwork.ValueChanged += SetWeaponModel;
    }

    private void OnDisable()
    {
        weaponNetwork.ValueChanged -= SetCurrentWeapon;
        playerClassNetwork.ValueChanged -= SetWeaponModel;
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

            var obj = Instantiate(weaponInfos[i].ThirdPersonModel, _weaponModelsRoot[i].transform);
            ModelHelper.SetOwnerModelSettings(playerClassNetwork, obj);
        }
    }
}