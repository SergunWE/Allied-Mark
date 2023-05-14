using NetworkFramework.NetcodeComponents;
using Unity.Netcode;
using UnityEngine;

public class WeaponNetwork : ObjectNetwork<int>
{
    public void SetWeaponIndex(int value)
    {
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