using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExitBoundsEvent : IEvent
{
    public readonly BaseProjectile Projectile;
    public readonly Vector3 ExitPosition; 
    public ProjectileExitBoundsEvent(BaseProjectile projectile, Vector3 exitPosition)
    {
        Projectile = projectile;
        ExitPosition = exitPosition;
    }
}
