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

        InstantiateBullet(position, currentColor);
        CurrentAmmo--;
        AudioManager.Instance?.PlayShootSFX();
        OnFire?.Invoke();
    }

    override protected void InstantiateBullet(Vector2 position, ColorData currentColor = default)
    {
        Projectile bullet = Object.Instantiate(WeaponData.ProjectilePrefab, position, rotation: Quaternion.identity);
        bullet.Instantiate(WeaponData.ProjectileSpeed, currentColor);
    }

    override public void RefillAmmo()
    {
        CurrentAmmo = WeaponData.MaxAmmo;
    }
}
