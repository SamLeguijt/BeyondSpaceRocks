using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTarget
{
    public void InteractWith(IProjectile projectile);
    public bool CanInteractWith(IProjectile projectile);
    public void OnProjectileHit(BaseProjectile projectile);
    
    public ColorData ColorData {get;}
}
