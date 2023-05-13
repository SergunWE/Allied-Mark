using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalk : NetworkBehaviour
{
    public float speed = 2f;
    private Rigidbody _rb;

    private float time;
    
    void Start()
    {
        if(!IsOwnedByServer) Destroy(this);
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveServerRpc();

        time += Time.deltaTime;
        if (time > 15)
        {
            time = 0;
            RotateServerRpc();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<NetworkBehaviour>(out var component))
        {
            if (component.NetworkObject.IsPlayerObject)
            {
                KillPlayerServerRpc(component.NetworkObject);
            }
        }
        else
        {
            RotateServerRpc();
        }
    }

    [ServerRpc]
    private void MoveServerRpc()
    {
        Vector3 movement = transform.forward * (speed * Time.deltaTime);
        _rb.MovePosition(_rb.position + movement);
    }
    
    [ServerRpc]
    private void RotateServerRpc()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 180));
    }

    [ServerRpc]
    private void KillPlayerServerRpc(NetworkObjectReference obj)
    {
        if (obj.TryGet(out var player))
        {
            player.Despawn();
        }
    }
}