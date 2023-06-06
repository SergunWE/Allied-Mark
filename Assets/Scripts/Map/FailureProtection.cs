using KinematicCharacterController;
using Unity.Netcode;
using UnityEngine;

public class FailureProtection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.TryGetComponent<NetworkObject>(out var networkObject)) return;
        if (!networkObject.IsOwner) return;
        networkObject.transform.GetComponent<KinematicCharacterMotor>().SetPosition(Vector3.zero);
    }
}