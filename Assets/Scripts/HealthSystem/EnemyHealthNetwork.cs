using Unity.Netcode;
using UnityEngine;

public class EnemyHealthNetwork : HealthNetwork
{
    [field: SerializeField] public EnemyInfo EnemyInfo { get; private set; }

    private void Start()
    {
        if (IsHost || IsServer)
        {
            NetworkVariable.Value = EnemyInfo.Health;
        }

        LocalValue = NetworkVariable.Value;
    }

    public override void TakeDamage(int damage, NetworkObject sender)
    {
        if (sender.IsPlayerObject)
        {
            TakeDamageServerRpc(damage);
        }
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

        if (result > EnemyInfo.Health)
        {
            result = EnemyInfo.Health;
        }

        NetworkVariable.Value = result;
    }

    protected override void VariableChangedMessage()
    {
        Debug.Log($"Enemy {OwnerClientId} have {LocalValue} HP");
    }
}