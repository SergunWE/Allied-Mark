using NetworkFramework.Netcode_Components;
using Unity.Netcode;
using UnityEngine;

public class WeaponNetwork : ObjectNetwork<int>
{
    private void Start()
    {
        TriggerEvent(NetworkVariable.Value);
    }

    [ServerRpc]
    public void SetWeaponIndexServerRpc(int value)
    {
        if (NetworkVariable.Value == value) return;
        NetworkVariable.Value = value;
        TriggerEvent(value);
    }

    protected override void OnVariableChanged(int oldValue, int newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log($"Client {OwnerClientId} set weapon index {newValue.ToString()}");
        TriggerEvent(newValue);
    }
}