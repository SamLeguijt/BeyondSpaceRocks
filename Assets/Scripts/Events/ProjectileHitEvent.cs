using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitEvent : IEvent
{
    public readonly IProjectile Projectile;
    public readonly IProjectileTarget Target;

    public ProjectileHitEvent(IProjectile projectile, IProjectileTarget hit)
    {
        Projectile = projectile;
        Target = hit; 
    }
}
