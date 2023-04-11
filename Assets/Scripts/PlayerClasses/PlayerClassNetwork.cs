using NetworkFramework.Data;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class PlayerClassNetwork : NetworkBehaviour
{
    [SerializeField] private PlayerClassViewer playerClassViewer;
    private readonly NetworkVariable<FixedString128Bytes> _playerClassName = new("");

    public override void OnNetworkSpawn()
    {
        _playerClassName.OnValueChanged += OnPlayerClassChanged;
    }

    private void Start()
    {
        string playerClassName;
        if (IsOwner)
        {
            playerClassName =
                LobbyData.GetPlayerData(CustomDataKeys.PlayerClass.Key, AuthenticationService.Instance.PlayerId);
            Debug.Log($"Owner {OwnerClientId} set player class");
            SetPlayerClassServerRpc(playerClassName);
        }
        else
        {
            playerClassName = _playerClassName.Value.Value;
            if (!string.IsNullOrEmpty(playerClassName))
            {
                playerClassViewer.SetPlayerModel(playerClassName);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        _playerClassName.OnValueChanged -= OnPlayerClassChanged;
    }

    [ServerRpc]
    private void SetPlayerClassServerRpc(string playerClassName)
    {
        _playerClassName.Value = new FixedString128Bytes(playerClassName);
    }

    private void OnPlayerClassChanged(FixedString128Bytes oldValue, FixedString128Bytes newValue)
    {
        if (oldValue == newValue) return;
        Debug.Log($"{OwnerClientId} set class {newValue}");
        playerClassViewer.SetPlayerModel(newValue.ToString());
    }
}