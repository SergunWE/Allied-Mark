using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

public class PlayerModelAnimationController : NetworkBehaviour
{
    private Animator _animator;

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
        if (_animator != null)
        {
            Debug.Log("Animator set up");
        }
        
    }
}