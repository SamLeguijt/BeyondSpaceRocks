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

        ProjectileData projectileData = GetModifiedProjectileData(position, currentColor);
        CreateBullet(projectileData);
        CurrentAmmo--;
        AudioManager.Instance?.PlayShootSFX();
        OnFire?.Invoke();
    }

    private ProjectileData GetModifiedProjectileData(Vector2 firePosition, ColorData currentColor)
    {
        ProjectileData data = WeaponData.ProjectileConfig.CreateRuntimeData();

        data.SpawnPosition = firePosition;
        data.MoveDirection = new Vector2(0, 1);
        data.ColorData = currentColor;

        //data.ApplyModifiers(); 

        return data;
    }

    override protected void CreateBullet(ProjectileData data)
    {
        BaseProjectile bullet = ProjectileSpawner.Spawn(data);
    }

    override public void RefillAmmo()
    {
        CurrentAmmo = WeaponData.MaxAmmo;
    }
}
