using System;
using NetworkFramework.NetcodeComponents;
using Unity.Netcode;

public abstract class HealthNetwork : ObjectNetwork<int>, IDamaged
{
    public event Action ObjectDeath;

    protected void SetHealth(int health)
    {
        LocalValue = health;
        SetHealthServerRpc(health);
    }
    
    [ServerRpc]
    private void SetHealthServerRpc(int health)
    {
        NetworkVariable.Value = health;
    }
    
    protected void Death()
    {
        ObjectDeath?.Invoke();
        DeathServerRpc();
    }

    [ServerRpc]
    private void DeathServerRpc()
    {
        NetworkObject.Despawn();
    }
    
    public abstract void TakeDamage(int damage, NetworkObject sender);
}