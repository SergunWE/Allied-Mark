using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(PlayerClassNetwork))]
public class PlayerHealthNetwork : HealthNetwork
{
    private PlayerClassNetwork _playerClassNetwork;

    private void Awake()
    {
        _playerClassNetwork = GetComponent<PlayerClassNetwork>();
    }

    private void Start()
    {
        if (!IsOwner) return;
        LocalValue = _playerClassNetwork.PlayerClassInfo.Health;
        SetHealthServerRpc(LocalValue);
    }

    [ServerRpc]
    private void SetHealthServerRpc(int health)
    {
        NetworkVariable.Value = health;
    }

    public override void TakeDamage(int damage, NetworkObject sender)
    {
        float damageCoef = sender.IsPlayerObject ? 0.2f : 1f;
        TakeDamageServerRpc((int) (damage * damageCoef));
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(int damage)
    {
        int result = NetworkVariable.Value - damage;
        if (result <= 0)
        {
            Death();
            return;
        }

        if (result > _playerClassNetwork.PlayerClassInfo.Health)
        {
            result = _playerClassNetwork.PlayerClassInfo.Health;
        }

        NetworkVariable.Value = result;
    }

    protected override void VariableChangedMessage()
    {
        Debug.Log($"Client {OwnerClientId} have {LocalValue} HP");
    }
}