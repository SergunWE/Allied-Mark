using Unity.Netcode;
using UnityEngine;

public class EnemyHealthNetwork : HealthNetwork
{
    [SerializeField] private MarkNetwork markNetwork;
    [SerializeField] private EnemyInfo enemyInfo;

    private void Start()
    {
        if (IsHost || IsServer)
        {
            NetworkVariable.Value = enemyInfo.Health;
        }

        LocalValue = NetworkVariable.Value;
    }

    public override void TakeDamage(int damage, NetworkObject sender)
    {
        if (!sender.IsPlayerObject) return;
        if(markNetwork.LocalMarks.Count > 0) Debug.Log("Have mark");
        int finalDamage = (int)(damage * (markNetwork.LocalMarks.Count > 0 ? 1.5f : 1f));
        TakeDamageServerRpc(finalDamage);
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

        if (result > enemyInfo.Health)
        {
            result = enemyInfo.Health;
        }

        NetworkVariable.Value = result;
    }

    protected override void VariableChangedMessage()
    {
        Debug.Log($"Enemy {OwnerClientId} have {LocalValue} HP");
    }
}