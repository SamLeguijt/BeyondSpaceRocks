using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem 
{
    private readonly IEventBus EventBus = null;
    private readonly CollisionResolver CollisionResolver = null;
    private readonly ProjectileFactory Factory = null;

    private List<IProjectile> activeProjectiles = new();

    public ProjectileSystem(IEventBus eventBus)
    {
        if (eventBus == null)
            throw new System.Exception();

        EventBus = eventBus;  
        CollisionResolver = new CollisionResolver();
        Factory = new ProjectileFactory();
    }
 
    public IProjectile GetProjectile(ProjectileData config)
    {
        IProjectile projectile = Factory.Create(config);
        activeProjectiles.Add(projectile);

        return projectile; 
    }

    private void RemoveProjectile(IProjectile projectile)
    {
        Factory.Return(projectile); 
        activeProjectiles.Remove(projectile);
    }

    public void HandleCollision(IProjectile projectile, Collision collision)
    {
        IProjectileTarget target = collision.gameObject.GetComponent<IProjectileTarget>();

        // Ignores non-specified target collisions
        if (target == null)  
            return;

        // Resolves runtime game rules to decide if an interaction should occur
        CollisionResult result = CollisionResolver.Resolve(projectile, target);

        // No interaction counts as a miss; projectile hit a target, but no interaction 
        if (!result.ShouldInteract)
        {
            // Fire event (miss)
            return; 
        }

        // Otherwise, projectiles hit a valid target: 

        target.OnProjectileHit(projectile);
        HandleProjectileCollisionResponse(projectile, projectile.CollisionResponse);
    }

    private void HandleProjectileCollisionResponse(IProjectile projectile, EProjectileCollisionResponse response)
    {
        /// Uses the response to decide what happens to the projectile
        switch (response)
        {
            case EProjectileCollisionResponse.DestroyOnImpact:
                RemoveProjectile(projectile);
                // fire event (hit)
            break; 
            case EProjectileCollisionResponse.Ignore:
            // do nothing
            // fire event (hit)
            break; 
        }   
    }

    // TODO: Call in update
    private void CheckProjectileBounds()
    {
        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            IProjectile projectile = activeProjectiles[i];

            if (OutOfBounds(projectile))
                HandleOutOfBounds(projectile);
        }
    }
    private bool OutOfBounds(IProjectile projectile)
    {
        // TODO: Implementation
        return false;
    }

    private void HandleOutOfBounds(IProjectile projectile)
    {
        EventBus.Publish(new ProjectileExitBoundsEvent(projectile, projectile.Position));
        RemoveProjectile(projectile);
    }
}
