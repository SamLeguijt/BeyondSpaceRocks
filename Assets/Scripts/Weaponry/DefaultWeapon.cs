using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : AbstractWeapon
{
    public DefaultWeapon(WeaponData weaponData) : base(weaponData)
    {
        WeaponData = weaponData; 
        CurrentAmmo = weaponData.MaxAmmo;
    }

    public override bool CanFire()
    {
        return HasAmmo();
    }

    public override bool HasAmmo()
    {
        return CurrentAmmo > 0; 
    }

    public override void Fire(Vector2 position, ColorData currentColor)
    {
        if (!CanFire())
            return;

        GetBullet(position, currentColor);
        CurrentAmmo--;
        AudioManager.Instance?.PlayShootSFX();
        OnFire?.Invoke();
    }

    override protected void GetBullet(Vector2 position, ColorData currentColor = default)
    {
        // BaseProjectile bullet = Object.Instantiate(WeaponData.ProjectilePrefab, position, rotation: Quaternion.identity);
        // bullet.Instantiate(WeaponData.ProjectileSpeed, currentColor);

        // TODO: U/se prpjectile system
    }

    override public void RefillAmmo()
    {
        CurrentAmmo = WeaponData.MaxAmmo;
    }
}
