using NetworkFramework;
using NetworkFramework.Data;
using NetworkFramework.Netcode_Components;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class PlayerClassNetwork : ObjectNetwork<FixedString128Bytes>
{
    //[SerializeField] private PlayerClassViewer playerClassViewer;

    private void Start()
    {
        string playerClassName = GetPlayerClassName();
        if (IsOwner)
        {
            Debug.Log($"Owner {OwnerClientId} set player class");
            SetPlayerClassServerRpc(playerClassName);
        }
        else
        {
            if (!string.IsNullOrEmpty(playerClassName))
            {
                TriggerEvent(playerClassName);
            }
        }
    }

    public string GetPlayerClassName()
    {
        return IsOwner ? LobbyData.GetPlayerData(CustomDataKeys.PlayerClass.Key, 
            AuthenticationService.Instance.PlayerId) : NetworkVariable.Value.Value;
    }

    [ServerRpc]
    private void SetPlayerClassServerRpc(string playerClassName)
    {
        if(NetworkVariable.Value == playerClassName) return;
        NetworkVariable.Value = new FixedString128Bytes(playerClassName);
        TriggerEvent(playerClassName);
    }

    protected override void OnVariableChanged(FixedString128Bytes oldValue, FixedString128Bytes newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log($"{OwnerClientId} set class {newValue}");
        TriggerEvent(newValue);
    }
}