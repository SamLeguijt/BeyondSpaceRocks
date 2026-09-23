using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissEvent : IEvent
{
    public readonly BaseProjectile Projectile;

    public ProjectileMissEvent(BaseProjectile projectile)
    {
        Projectile = projectile;
    }
}
