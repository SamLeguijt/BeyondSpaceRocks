using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileSystem : IProjectileSpawner
{
    private readonly IEventBus EventBus = null;
    private readonly InteractionResolver InteractionResolver = null;
    private readonly ProjectileFactory Factory = null;
    private List<BaseProjectile> activeProjectiles = new();

    public ProjectileSystem(IEventBus eventBus)
    {
        if (eventBus == null)
            throw new System.Exception();

        EventBus = eventBus;  
        InteractionResolver = new InteractionResolver();
        Factory = new ProjectileFactory();
    }

    public BaseProjectile Spawn(ProjectileData data)
    {
        BaseProjectile projectile = Factory.Create(data);
        projectile.ProjectileCollisionEvent += HandleCollision;
        activeProjectiles.Add(projectile);
        projectile.Activate();
        return projectile;
    }

    public void Despawn(BaseProjectile projectile)
    {
        activeProjectiles.Remove(projectile);
        projectile.ProjectileCollisionEvent -= HandleCollision;
        Factory.Return(projectile);
    }

    public void HandleCollision(BaseProjectile projectile, Collider2D collision)
    {
        IProjectileTarget target = collision.gameObject.GetComponent<IProjectileTarget>();

        // Ignores non-specified target collisions
        if (target == null)  
            return;

        // Resolves runtime game rules to decide if an interaction should occur
        InteractionResult result = InteractionResolver.Resolve(projectile, target);

        // No interaction counts as a miss; projectile hit a target, but no interaction 
        if (!result.ShouldInteract)
        {
            EventBus.Publish(new ProjectileMissEvent(projectile));
            return; 
        }

        // Otherwise, projectiles hit a valid target: 

        target.OnProjectileHit(projectile);
        EventBus.Publish<ProjectileHitEvent>(new ProjectileHitEvent(projectile, target));
        HandleProjectileCollisionResponse(projectile);
    }

    private void HandleProjectileCollisionResponse(BaseProjectile projectile)
    {
        /// Uses the response to decide what happens to the projectile
        switch (projectile.Data.CollisionResponse)
        {
            case EProjectileCollisionResponse.Ignore:
                break;
            case EProjectileCollisionResponse.DestroyOnImpact:
                Despawn(projectile);
            break; 
        }   
    }

    // TODO: Call in update
    private void CheckProjectileBounds()
    {
        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            BaseProjectile projectile = activeProjectiles[i];

            if (OutOfBounds(projectile))
                HandleOutOfBounds(projectile);
        }
    }

    private bool OutOfBounds(BaseProjectile projectile)
    {
        // TODO: Implementation
        return false;
    }

    private void HandleOutOfBounds(BaseProjectile projectile)
    {
        EventBus.Publish(new ProjectileExitBoundsEvent(projectile, projectile.transform.position));
        Despawn(projectile);
    }


}
