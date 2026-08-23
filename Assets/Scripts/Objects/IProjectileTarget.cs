using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTarget
{
    public void OnProjectileCollision(Projectile projectile);
    public bool CanReceiveHit(Projectile projectile);
}
