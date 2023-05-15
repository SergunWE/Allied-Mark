using NetworkFramework;
using NetworkFramework.Data;
using NetworkFramework.NetcodeComponents;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class PlayerClassNetwork : ObjectNetwork<FixedString128Bytes>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;

    private void Start()
    {
        LocalValue = PlayerClassName;
        if (IsOwner)
        {
            SetPlayerClassServerRpc(LocalValue);
        }
    }

    private string PlayerClassName => IsOwner
        ? LobbyData.GetPlayerData(CustomDataKeys.PlayerClass.Key, AuthenticationService.Instance.PlayerId)
        : NetworkVariable.Value.Value;


    public PlayerClass PlayerClassInfo => string.IsNullOrEmpty(PlayerClassName)
        ? null
        : playerClassHandler.DataDictionary[PlayerClassName];

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