using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAttack : NetworkBehaviour
{
    [SerializeField] private EnemyInfo enemyInfo;

    private int _damage;
    private IEnemyBehavior _enemyBehavior;

    private void Awake()
    {
        _damage = enemyInfo.Damage;
        _enemyBehavior = GetComponent<IEnemyBehavior>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var comp = ComponentHelper.FindComponent<PlayerHealthNetwork>(collision.collider.gameObject);
        if (comp != null && IsOwner)
        {
            comp.TakeDamage(_damage, NetworkObject);
            _enemyBehavior?.PlayerAttacked(comp);
        }
    }
}