using System;
using UnityEngine;

public abstract class AbstractWeapon 
{
    public WeaponData WeaponData { get; protected set; } = null;
    protected IProjectileSpawner ProjectileSpawner { get; private set; } = null;
    public int CurrentAmmo { get; set; } = 0;
    public Action OnFire {  get; protected set; } = null;
    public Transform Firepoint { get; protected set; }


    public AbstractWeapon(WeaponData weaponData, IProjectileSpawner projectileSpawner, Transform firepoint) 
    {
        WeaponData = weaponData;
        ProjectileSpawner = projectileSpawner;
        Firepoint = firepoint;
    }

    public abstract bool CanFire(); 

    public abstract bool HasAmmo();
    
    public abstract void Fire(ColorData currentColor);

    protected abstract void CreateBullet(ProjectileData data); 

    public abstract void RefillAmmo(); 
}
