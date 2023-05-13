using NetworkFramework;
using NetworkFramework.Data;
using NetworkFramework.Netcode_Components;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class PlayerClassNetwork : ObjectNetwork<FixedString128Bytes>
{
    private void Start()
    {
        LocalValue = GetPlayerClassName();
        if (IsOwner)
        {
            SetPlayerClassServerRpc(LocalValue);
        }
        // string playerClassName = GetPlayerClassName();
        // if (IsOwner)
        // {
        //     SetPlayerClassServerRpc(playerClassName);
        // }
        // else
        // {
        //     if (!string.IsNullOrEmpty(playerClassName))
        //     {
        //         LocalValue = playerClassName;
        //     }
        // }
    }

    public string GetPlayerClassName()
    {
        return IsOwner ? LobbyData.GetPlayerData(CustomDataKeys.PlayerClass.Key, 
            AuthenticationService.Instance.PlayerId) : NetworkVariable.Value.Value;
    }

    [ServerRpc]
    private void SetPlayerClassServerRpc(FixedString128Bytes playerClassName)
    {
        NetworkVariable.Value = playerClassName;
    }

    protected override void VariableChangedMessage()
    {
        Debug.Log($"{OwnerClientId} set class {LocalValue}");
    }
}