using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Info")]
public class WeaponInfo : ScriptableObject
{
    public string weaponName;
    
    public bool isAuto;
    public bool isShotgun;
    public bool haveSniperScope;

    public int clipSize;

    public GameObject model;
}
