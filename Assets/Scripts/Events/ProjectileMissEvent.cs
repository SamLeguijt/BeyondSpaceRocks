using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissEvent : IEvent
{
    public readonly IProjectile Projectile;

    public ProjectileMissEvent(IProjectile projectile)
    {
        Projectile = projectile;
    }
}
