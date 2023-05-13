public class Weapon
{
    public WeaponInfo WeaponInfo { get; }
    public int CurrentBullets { get; set; }
    public WeaponState State { get; set; }

    public Weapon(WeaponInfo weaponInfo)
    {
        WeaponInfo = weaponInfo;
        CurrentBullets = weaponInfo.ClipSize;
        State = WeaponState.NotReady;
    }
}