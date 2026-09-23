using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProjectileCollisionResponse
{
    DestroyOnImpact,
    Ignore, 
    // Bounce etc?
}

public struct CollisionResult
{
    public IProjectile Projectile {get;}
    public IProjectileTarget Target {get;}
    public bool ShouldInteract { get;}

    public CollisionResult(IProjectile projectile, IProjectileTarget target, bool interact) 
    {
        Projectile = projectile;
        Target = target; 
        ShouldInteract = interact; 
    }
}

public class CollisionResolver 
{
    public CollisionResult Resolve(IProjectile projectile, IProjectileTarget target)
    {
        bool shouldInteract = false;

        switch (projectile.InteractionRule)
        {
            case EInteractionRule.MatchColor:
                if (projectile.ColorData.ColorType == target.ColorData.ColorType)
                    shouldInteract = true;
            break;
            case EInteractionRule.IgnoreColor:
                shouldInteract = true;

            break; 
        }

        return new CollisionResult(projectile, target, shouldInteract);
    }
}
