using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class EnemyRandomWalk : NetworkBehaviour, IEnemyBehavior
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float walkTime = 15f;

    [SerializeField] private float playerPushMultiply = 1f;

    private Rigidbody _rb;
    private float _time;

    private void Start()
    {
        if (!IsHost || !IsServer) Destroy(this);
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveServerRpc();

        _time += Time.deltaTime;
        if (!(_time > walkTime)) return;
        _time = 0;
        RotateServerRpc();
    }

    private void OnCollisionEnter(Collision collision)
    {
        RotateServerRpc();
    }

    [ServerRpc]
    private void MoveServerRpc()
    {
        var movement = transform.forward * (speed * Time.deltaTime);
        _rb.MovePosition(_rb.position + movement);
    }

    [ServerRpc]
    private void RotateServerRpc()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 180));
    }

    [ServerRpc]
    private void LookAtServerRpc(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        targetDirection.y = 0; // set the y component to 0 so that the object only rotates along the y-axis
        transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    public void PlayerAttacked(NetworkBehaviour player)
    {
        LookAtServerRpc(player.transform.position);
        var comp = ComponentHelper.FindComponent<MovementCharacterController>(player.transform.parent.gameObject);
        if (comp == null) return;
        var direction = (comp.transform.position - transform.position).normalized * playerPushMultiply;
        var vector3 = new Vector3(direction.x, direction.y < 0 ? 0 : direction.y, direction.z);
        comp.AddVelocity(vector3);
    }
}