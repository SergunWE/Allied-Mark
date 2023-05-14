using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponAnimationController : MonoBehaviour
{
    private Animator _animator;
    private static readonly int PullSpeed = Animator.StringToHash("PullSpeed");
    private static readonly int ShootSpeed = Animator.StringToHash("ShootSpeed");
    private static readonly int ReloadSpeed = Animator.StringToHash("ReloadSpeed");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnPullingSpeedChanged(float value)
    {
        _animator.SetFloat(PullSpeed, value);
    }
    
    public void OnShootingSpeedChanged(float value)
    {
        _animator.SetFloat(ShootSpeed, value);
    }
    
    public void OnReloadingSpeedChanged(float value)
    {
        _animator.SetFloat(ReloadSpeed, value);
    }

    public void OnWeaponChanged()
    {
        _animator.StopPlayback();
        _animator.Play("FirstPersonWeaponPulling");
    }

    public void OnWeaponShooting()
    {
        _animator.StopPlayback();
        _animator.Play("FirstPersonWeaponShooting");
    }

    public void OnWeaponReloading(bool state)
    {
        if (!state) return;
        _animator.StopPlayback();
        _animator.Play("FirstPersonWeaponReloading");
    }
}