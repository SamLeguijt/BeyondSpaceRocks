using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory 
{
    private IProjectileSpawner projectileSpawner; 
    
    public WeaponFactory(IProjectileSpawner projectileSpawner)
    {
        this.projectileSpawner = projectileSpawner;
    }

    public AbstractWeapon Create(WeaponData data)
    {
        return new DefaultWeapon(data, projectileSpawner);
    }
}
