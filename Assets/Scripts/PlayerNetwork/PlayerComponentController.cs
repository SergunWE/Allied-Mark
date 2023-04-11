using Unity.Netcode;
using UnityEngine;

public class PlayerComponentController : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;
        Destroy(characterController);
        Destroy(gameObject);
    }
}
