using Unity.Netcode;

public interface IDamaged
{
    void TakeDamage(int damage, NetworkObject sender);
}