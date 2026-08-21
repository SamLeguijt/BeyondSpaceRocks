using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon 
{
    public WeaponData WeaponData { get; protected set; } = null;

    protected int CurrentAmmo { get; set; } = 0;
    
    public Action OnFire {  get; protected set; } = null;

    public AbstractWeapon(WeaponData weaponData) { }

    public abstract bool CanFire(); 

    public abstract bool HasAmmo();
    
    public abstract void Fire(Vector2 position, ColorData currentColor);

    protected abstract void InstantiateBullet(Vector2 position, ColorData currentColor = default); 

    public abstract void RefillAmmo(); 
}
