using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WeaponViewer : MonoBehaviour
{
    [SerializeField] private WeaponNetwork weaponNetwork;
    [SerializeField] private PlayerClassNetwork playerClassNetwork;
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private List<GameObject> weaponModelsRoot;

    private int _localIndex = -1;
    private GameObject _prevCurrentWeaponModel;

    private void Awake()
    {
        foreach (var model in weaponModelsRoot)
        {
            model.SetActive(false);
        }
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
        if (_localIndex == index) return;
        _localIndex = index;
        if (_prevCurrentWeaponModel != null)
        {
            _prevCurrentWeaponModel.SetActive(false);
        }

        var currentModel = weaponModelsRoot[index];
        currentModel.SetActive(true);
        _prevCurrentWeaponModel = currentModel;
    }

    private void SetWeaponModel(FixedString128Bytes playerClassName)
    {
        var weaponInfos = playerClassHandler.DataDictionary[playerClassName.Value].weapons;
        for (int i = 0; i < weaponInfos.Count; i++)
        {
            foreach (Transform children in weaponModelsRoot[i].transform)
            {
                Destroy(children.gameObject);
            }

            Instantiate(weaponInfos[i].model, weaponModelsRoot[i].transform);
        }
    }
}