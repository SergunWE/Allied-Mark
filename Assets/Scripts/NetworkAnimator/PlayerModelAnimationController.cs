using KinematicCharacterController;
using Unity.Netcode;
using UnityEngine;

public class PlayerModelAnimationController : NetworkBehaviour
{
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private KinematicCharacterMotor characterMotor;

    private Animator _animator;
    private bool _animatorSet;
    
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
        if (_animator == null) return;
        Debug.Log("Animator set up");
        _animator.runtimeAnimatorController = animatorController;
        _animatorSet = true;
    }

    private void FixedUpdate()
    {
        if (!_animatorSet || !IsOwner) return;
        _animator.SetBool(IsRunning, characterMotor.Velocity.magnitude > 0.1f);
    }
}