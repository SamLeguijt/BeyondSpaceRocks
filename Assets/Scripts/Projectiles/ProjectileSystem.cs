using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem : IProjectileSpawner, IUpdatable
{
    private readonly IEventBus eventBus = null;
    private readonly InteractionResolver interactionResolver = null;
    private readonly ProjectileFactory factory = null;
    private readonly Bounds playfieldBounds;
    private List<BaseProjectile> activeProjectiles = new();

    public ProjectileSystem(IEventBus eventBus, Bounds playfield)
    {
        if (eventBus == null)
            throw new System.Exception("[ProjectileSystem] Event bus is null");

        this.eventBus = eventBus;  
        playfieldBounds = playfield;
        interactionResolver = new InteractionResolver();
        factory = new ProjectileFactory();
    }

    public BaseProjectile Spawn(ProjectileData data)
    {
        BaseProjectile projectile = factory.Get(data);
        projectile.ProjectileCollisionEvent += HandleCollision;
        activeProjectiles.Add(projectile);
        projectile.Activate();
        return projectile;
    }

    public void Despawn(BaseProjectile projectile)
    {
        if (!activeProjectiles.Contains(projectile))
            throw new System.Exception();

        projectile.ProjectileCollisionEvent -= HandleCollision;
        activeProjectiles.Remove(projectile);
        factory.Return(projectile);
    }

    public void HandleCollision(BaseProjectile projectile, Collider2D collision)
    {
        IProjectileTarget target = collision.gameObject.GetComponent<IProjectileTarget>();

        // Ignores non-target collisions
        if (target == null)  
            return;

        // Resolves runtime game rules to decide if an interaction should occur
        InteractionResult result = interactionResolver.Resolve(projectile, target);

        // No interaction counts as a miss; projectile hit a target, but no interaction 
        if (!result.ShouldInteract)
        {
            eventBus.Publish(new ProjectileMissEvent(projectile));
            return; 
        }

        // Otherwise, projectiles hit a valid target: 
        target.OnProjectileHit(projectile);
        eventBus.Publish(new ProjectileHitEvent(projectile, target));
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

    public void Update(float deltaTime)
    {
        CheckProjectileBounds();
    }

    private void CheckProjectileBounds()
    {
        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            BaseProjectile projectile = activeProjectiles[i];

            if (!IsInPlayfield(projectile))
                HandleOutOfBounds(projectile);
        }
    }

    /// TODO: Make static utils method? 
    private bool IsInPlayfield(BaseProjectile projectile)
    {
        Vector2 position = projectile.transform.position;

        return position.x >= playfieldBounds.min.x &&
               position.x <= playfieldBounds.max.x &&
               position.y >= playfieldBounds.min.y &&
               position.y <= playfieldBounds.max.y;
    }

    private void HandleOutOfBounds(BaseProjectile projectile)
    {
        eventBus.Publish(new ProjectileExitBoundsEvent(projectile, projectile.transform.position));
        Despawn(projectile);
    }
}
