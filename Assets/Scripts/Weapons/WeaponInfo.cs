using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Info")]
public class WeaponInfo : ScriptableObject
{
    public string weaponName;
    
    public WeaponBehaviorBase weaponBehaviorBase = new SingleShotWeapon();

    public int clipSize;

    public GameObject model;
}
