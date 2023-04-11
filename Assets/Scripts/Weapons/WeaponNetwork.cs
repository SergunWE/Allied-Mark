using Unity.Netcode;
using UnityEngine;

public class WeaponNetwork : NetworkBehaviour
{
    [SerializeField] private WeaponViewer weaponViewer;
    
    private readonly NetworkVariable<int> _currentWeaponIndex = new(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        _currentWeaponIndex.OnValueChanged += OnCurrentWeaponChanged;
    }

    public override void OnNetworkDespawn()
    {
        _currentWeaponIndex.OnValueChanged -= OnCurrentWeaponChanged;
    }
    
    public void SetCurrentWeapon(int index)
    {
        if (index == _currentWeaponIndex.Value) return;
        weaponViewer.SetCurrentWeapon(_currentWeaponIndex.Value);
        _currentWeaponIndex.Value = index;
    }

    private void OnCurrentWeaponChanged(int oldValue, int newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log($"Client {OwnerClientId} set weapon index {newValue.ToString()}");
        weaponViewer.SetCurrentWeapon(newValue);
    }
}