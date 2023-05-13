using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Info")]
public class WeaponInfo : ScriptableObject
{
    [field: SerializeField] public string WeaponName { get; private set; }

    [field: SerializeField] public GameObject ThirdPersonModel { get; private set; }
    [field: SerializeField] public GameObject FirstPersonModel { get; private set; }

    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int BulletsPerShot { get; private set; }
    [field: SerializeField] public int ClipSize { get; private set; }
    [field: SerializeField] public float Recoil { get; private set; }
    [field: SerializeField] public float Spread { get; private set; }
    [field: SerializeField] public float ScopeZoom { get; private set; }
    [field: SerializeField] public float PullTime { get; private set; }
    [field: SerializeField] public float ShootTime { get; private set; }
    [field: SerializeField] public float ReloadTime { get; private set; }
}