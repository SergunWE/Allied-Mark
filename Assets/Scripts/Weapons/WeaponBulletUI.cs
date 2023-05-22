using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

public class WeaponBulletUI : MonoBehaviour
{
	[SerializeField] private CurrentPlayerWeapon currentPlayerWeapon;

	[SerializeField] private GameEventString bulletChanged;
	[SerializeField] private GameEventString clipSizeChanged;

	private Weapon CurrentWeapon => currentPlayerWeapon.CurrentWeapon.Weapon;

	public void OnWeaponShooting()
	{
		bulletChanged.Raise(CurrentWeapon.CurrentBullets.ToString());
	}

	public void OnWeaponReloading(bool state)
	{
		bulletChanged.Raise(CurrentWeapon.CurrentBullets.ToString());
	}

	public void OnWeaponChanged()
	{
		bulletChanged.Raise(CurrentWeapon.CurrentBullets.ToString());
		clipSizeChanged.Raise(CurrentWeapon.WeaponInfo.ClipSize.ToString());
	}
}