using System;
using UnityEngine;

public abstract class AbstractWeapon 
{
    public WeaponData WeaponData { get; protected set; } = null;
    protected IProjectileSpawner ProjectileSpawner { get; private set; } = null;
    public int CurrentAmmo { get; set; } = 0;
    public Action OnFire {  get; protected set; } = null;


    public AbstractWeapon(WeaponData weaponData, IProjectileSpawner projectileSpawner) 
    {
        WeaponData = weaponData;
        ProjectileSpawner = projectileSpawner;
    }

    public abstract bool CanFire(); 

    public abstract bool HasAmmo();
    
    public abstract void Fire(Vector2 position, ColorData currentColor);

    protected abstract void CreateBullet(ProjectileData data); 

    public abstract void RefillAmmo(); 
}
