using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTarget
{
    public void InteractWith(Projectile projectile);
    public bool CanInteractWith(Projectile projectile);
}
