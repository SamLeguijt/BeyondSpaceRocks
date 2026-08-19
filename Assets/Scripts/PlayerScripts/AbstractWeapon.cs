using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon 
{
    protected Projectile Projectile { get; set; }

    public float ProjectileSpeed { get; protected set; }

    public Timer CooldownTimer {  get; protected set; }
    
    public float CooldownSeconds { get; protected set; }

    public int MaxAmmo { get; protected set; }

    protected int CurrentAmmo { get; set; }
    
    public Action OnFire { get; protected set; }

    public abstract bool CanFire(); 
    
    public abstract void Fire(Vector2 position, ColorData currentColor);

    protected abstract void InstantiateBullet(ColorData currentColor = default); 

    public abstract void Reload(); 
}
