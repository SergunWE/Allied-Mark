using KinematicCharacterController;
using Unity.Netcode;

public class PlayerComponentManager : NetworkBehaviour
{
    private void Start()
    {
        if (IsOwner) return;
        Destroy(GetComponent<CharacterController>());
        Destroy(GetComponent<KinematicCharacterMotor>());
    }
}
