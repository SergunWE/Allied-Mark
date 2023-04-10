using Unity.Netcode;
using UnityEngine;

public class WeaponViewer : NetworkBehaviour
{
    [SerializeField] private GameObject mainWeaponModel;
    [SerializeField] private GameObject ancillaryWeaponModel;

    private uint _localIndex = uint.MaxValue;
    
    public void SetWeaponModel(uint index)
    {
        if(_localIndex == index) return;
        mainWeaponModel.SetActive(index == 0);
        ancillaryWeaponModel.SetActive(index != 0);
        _localIndex = index;
    }
}