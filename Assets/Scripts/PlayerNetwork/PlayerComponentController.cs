using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerComponentController : NetworkBehaviour
{
    [FormerlySerializedAs("characterController")] [SerializeField] private MovementCharacterController movementCharacterController;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;
        Destroy(movementCharacterController);
        Destroy(gameObject);
    }
}
