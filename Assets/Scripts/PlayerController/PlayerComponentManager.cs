using KinematicCharacterController;
using Unity.Netcode;

public class PlayerComponentManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;
        Destroy(GetComponent<CharacterController>());
        Destroy(GetComponent<KinematicCharacterMotor>());
    }
}
