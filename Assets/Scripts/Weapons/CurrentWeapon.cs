using Unity.Netcode;
using UnityEngine;

public class CurrentWeapon : NetworkBehaviour
{
    [SerializeField] private WeaponViewer weaponViewer;
    
    private readonly NetworkVariable<uint> _currentWeaponIndex = new(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    public override void OnNetworkSpawn()
    {
        _currentWeaponIndex.OnValueChanged += OnCurrentWeaponChanged;
        weaponViewer.SetWeaponModel(_currentWeaponIndex.Value);
    }

    public override void OnNetworkDespawn()
    {
        _currentWeaponIndex.OnValueChanged -= OnCurrentWeaponChanged;
    }
    
    public void SetCurrentWeapon(uint index)
    {
        if (index == _currentWeaponIndex.Value) return;
        weaponViewer.SetWeaponModel(_currentWeaponIndex.Value);
        _currentWeaponIndex.Value = index;
    }

    private void OnCurrentWeaponChanged(uint oldValue, uint newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log($"Client {OwnerClientId} set weapon index {newValue.ToString()}");
        weaponViewer.SetWeaponModel(newValue);
    }
}