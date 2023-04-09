using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class CurrentWeapon : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> weaponName = new("", NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        weaponName.OnValueChanged += OnCurrentWeaponChanged;
    }

    public override void OnNetworkDespawn()
    {
        weaponName.OnValueChanged -= OnCurrentWeaponChanged;
    }

    private void OnCurrentWeaponChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log(OwnerClientId + newValue.ToString());
    }
}