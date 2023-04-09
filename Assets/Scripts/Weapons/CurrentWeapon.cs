using Unity.Netcode;
using UnityEngine;

public class CurrentWeapon : NetworkBehaviour
{
    public NetworkVariable<uint> currentWeaponIndex = new(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        currentWeaponIndex.OnValueChanged += OnCurrentWeaponChanged;
    }

    public override void OnNetworkDespawn()
    {
        currentWeaponIndex.OnValueChanged -= OnCurrentWeaponChanged;
    }

    private void OnCurrentWeaponChanged(uint oldValue, uint newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log($"Client {OwnerClientId} set weapon index {newValue.ToString()}");
    }
}