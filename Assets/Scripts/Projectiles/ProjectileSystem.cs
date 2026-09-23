using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem 
{
    private readonly IEventBus EventBus = null;
    private readonly CollisionResolver CollisionResolver = null;

    public ProjectileSystem(IEventBus eventBus)
    {
        if (eventBus == null)
            throw new System.Exception();

        EventBus = eventBus;  
        CollisionResolver = new CollisionResolver();
    }

 
    public void HandleCollision(IProjectile projectile, Collision collision)
    {
        IProjectileTarget target = collision.gameObject.GetComponent<IProjectileTarget>();

        if (target == null)  
            return;

        CollisionResult result = CollisionResolver.Resolve(projectile, target);

        if (!result.ShouldInteract)
        {
            // Fire event (miss)
            return; 
        }

        HandleProjectileCollisionResponse(projectile, projectile.CollisionResponse);

        // if (target.CanInteractWith(projectile))
        // {
        //     target.InteractWith(projectile);

        //     EventBus.Publish<ProjectileHitEvent>(new ProjectileHitEvent(projectile, target));
        //     //OnCollision();
        // }
        // else
        // {
        //     EventBus.Publish(new ProjectileMissEvent(projectile));
        // }
    }

    private void HandleProjectileCollisionResponse(IProjectile projectile, EProjectileCollisionResponse response)
    {
        switch (response)
        {
            case EProjectileCollisionResponse.DestroyOnImpact:
            // return to pool 
            // fire event (hit)
            break; 
            case EProjectileCollisionResponse.Ignore:
            // do nothing
            // fire event (hit)
            break; 
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
