using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProjectileCollisionResponse
{
    DestroyOnImpact,
    Ignore, 
    // Bounce etc?
}

public struct InteractionResult
{
    public BaseProjectile Projectile {get;}
    public IProjectileTarget Target {get;}
    public bool ShouldInteract { get;}

    public InteractionResult(BaseProjectile projectile, IProjectileTarget target, bool interact) 
    {
        Projectile = projectile;
        Target = target; 
        ShouldInteract = interact; 
    }
}

public class InteractionResolver 
{
    public InteractionResult Resolve(BaseProjectile projectile, IProjectileTarget target)
    {
        bool shouldInteract = false;

        switch (projectile.Data.InteractionRule)
        {
            case EInteractionRule.MatchColor:
                if (projectile.Data.ColorData.ColorType == target.ColorData.ColorType)
                    shouldInteract = true;
            break;
            case EInteractionRule.IgnoreColor:
                shouldInteract = true;

            break; 
        }

        return new InteractionResult(projectile, target, shouldInteract);
    }
}
