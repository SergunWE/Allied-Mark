using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InstructionZone : MonoBehaviour
{
    [SerializeField] private GameEvent instructionShowed;
    [SerializeField] private GameEvent instructionHided;
    [SerializeField] private GameObject instruction;
    private bool _isInstructionShowing;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<NetworkObject>().IsOwner) return;
        instructionShowed.Raise();
        instruction.SetActive(true);
        _isInstructionShowing = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideInstruction()
    {
        if(!_isInstructionShowing) return;
        _isInstructionShowing = false;
        instruction.SetActive(false);
        instructionHided.Raise();
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }
}