using Unity.Netcode;

public interface IEnemyBehavior
{
    public void PlayerAttacked(NetworkBehaviour player);
}