using KinematicCharacterController;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerModelAnimationController : NetworkBehaviour
{
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private KinematicCharacterMotor characterMotor;

    [SerializeField] private Animator animator;

    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    // public void SetAnimator(Animator animator)
    // {
    //     this.animator = animator;
    //     if (this.animator == null) return;
    //     Debug.Log("Animator set up");
    //     this.animator.runtimeAnimatorController = animatorController;
    //     _animatorSet = true;
    // }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        animator.SetBool(IsRunning, characterMotor.Velocity.magnitude > 0.1f);
    }
}