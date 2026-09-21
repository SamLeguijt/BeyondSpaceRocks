using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem 
{
    private readonly IEventBus EventBus = null;

    public ProjectileSystem(IEventBus eventBus)
    {
        if (eventBus == null)
            throw new System.Exception();

        EventBus = eventBus;  
    }

 
    public void HandleCollision(IProjectile projectile, Collision collision)
    {
        //IProjectileTarget target = collision.GetComponent<IProjectileTarget>();

        //if (target == null)
        //    return;

        //if (target.CanInteractWith(this))
        //{
        //    target.InteractWith(this);
        //    //OnCollision();
        //}
    }
}
