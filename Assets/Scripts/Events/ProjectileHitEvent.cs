using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitEvent : IEvent
{
    public readonly BaseProjectile Projectile;
    public readonly IProjectileTarget Target;

    public ProjectileHitEvent(BaseProjectile projectile, IProjectileTarget hit)
    {
        Projectile = projectile;
        Target = hit; 
    }
}
