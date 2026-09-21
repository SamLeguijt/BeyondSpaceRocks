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
        IProjectileTarget target = collision.gameObject.GetComponent<IProjectileTarget>();

        if (target == null)  
            return;

        if (target.CanInteractWith(projectile))
        {
            target.InteractWith(projectile);

            EventBus.Publish<ProjectileHitEvent>(new ProjectileHitEvent(projectile, target));
            //OnCollision();
        }
        else
        {
            EventBus.Publish(new ProjectileMissEvent(projectile));
        }
    }

    private void CheckBounds(IProjectile projectile)
    {
        //if projectile.Position == Outputter of bounds
        //HandleOutOfBounds(projectile);
    }

    private void HandleOutOfBounds(IProjectile projectile)
    {
        EventBus.Publish(new ProjectileExitBoundsEvent(projectile, projectile.Position));
    }
}
