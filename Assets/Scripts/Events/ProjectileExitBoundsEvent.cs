using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExitBoundsEvent : IEvent
{
    public readonly IProjectile Projectile;
    public readonly Vector3 ExitPosition; 
    public ProjectileExitBoundsEvent(IProjectile projectile, Vector3 exitPosition)
    {
        Projectile = projectile;
        ExitPosition = exitPosition;
    }
}
