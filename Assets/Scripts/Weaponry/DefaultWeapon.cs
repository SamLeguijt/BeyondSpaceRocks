using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : AbstractWeapon
{
    public DefaultWeapon(WeaponData weaponData, IProjectileSpawner spawner) : base(weaponData, spawner)
    {
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

        CreateBullet(position, currentColor);
        CurrentAmmo--;
        AudioManager.Instance?.PlayShootSFX();
        OnFire?.Invoke();
    }

    private ProjectileData GetModifiedProjectileData()
    {
        ProjectileData data = WeaponData.ProjectileConfig.CreateRuntimeData();
        //data.ApplyModifiers(); TODO: Use a ColorModifier to set the color to weapon color
        return data;
    }

    override protected void CreateBullet(Vector2 position, ColorData currentColor = default)
    {
        // BaseProjectile bullet = Object.Instantiate(WeaponData.ProjectilePrefab, position, rotation: Quaternion.identity);
        // bullet.Instantiate(WeaponData.ProjectileSpeed, currentColor);

        // TODO: U/se prpjectile system
        ProjectileData data = GetModifiedProjectileData();
        BaseProjectile bullet = ProjectileSpawner.Spawn(data);
    }

    override public void RefillAmmo()
    {
        CurrentAmmo = WeaponData.MaxAmmo;
    }
}
