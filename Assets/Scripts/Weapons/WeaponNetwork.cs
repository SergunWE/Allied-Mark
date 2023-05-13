using NetworkFramework.Netcode_Components;
using Unity.Netcode;
using UnityEngine;

public class WeaponNetwork : ObjectNetwork<int>
{
    public void SetWeaponIndex(int value)
    {
        if(value == NetworkVariable.Value) return;
        LocalValue = value;
        SetWeaponIndexServerRpc(value);
    }
    
    [ServerRpc]
    private void SetWeaponIndexServerRpc(int value)
    {
        NetworkVariable.Value = value;
    }

    protected override void VariableChangedMessage()
    {
        Debug.Log($"Client {OwnerClientId} set weapon index {LocalValue.ToString()}");
    }
}