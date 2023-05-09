using Unity.Netcode;
using UnityEngine;

public class PlayerComponentController : NetworkBehaviour
{
    [SerializeField] private MovementCharacterController movementCharacterController;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;
        Destroy(movementCharacterController);
        Destroy(gameObject);
    }
}
