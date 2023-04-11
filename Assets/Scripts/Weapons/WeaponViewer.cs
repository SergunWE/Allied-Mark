using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponViewer : NetworkBehaviour
{
    [SerializeField] private List<GameObject> weaponModelsRoot;

    private int _localIndex;
    private GameObject _prevCurrentWeaponModel;
    
    public void SetCurrentWeapon(int index)
    {
        if(_localIndex == index) return;
        _localIndex = index;
        if (_prevCurrentWeaponModel != null)
        {
            _prevCurrentWeaponModel.SetActive(false);
        }

        var currentModel = weaponModelsRoot[index];
        currentModel.SetActive(true);
        _prevCurrentWeaponModel = currentModel;
    }

    public void SetWeaponModel(int index, GameObject weaponModel)
    {
        foreach (Transform children in weaponModelsRoot[index].transform)
        {
            Destroy(children.gameObject);
        }

        Instantiate(weaponModel, weaponModelsRoot[index].transform);
    }
}