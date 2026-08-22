using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    [field: SerializeField] public HitPointComponent HP { get; private set; } 
    [field: SerializeField] public AbstractColorHandler ColorInteractionHandler { get; protected set; }
    public delegate void DestructionEventHandler(DestructibleObject destroyed, Projectile by); 
    public event DestructionEventHandler OnDestroyEvent; 

    public void OnProjectileCollision(Projectile projectile) 
    {
        if (!CanHandleProjectileCollision(projectile))
            return;

        HandleCollision(projectile);
    }

    protected virtual bool CanHandleProjectileCollision(Projectile projectile)
    {
        return HP.CanTakeHit() && 
                ColorInteractionHandler.ResolveColorInteraction(projectile.ColorData);
    }

    protected virtual void HandleCollision(Projectile projectile)
    {
        HP.TakeHitPoints(1);

        if (!HP.IsAlive)
        {
            OnDestruct(projectile);
        }
    }

    protected virtual void OnDestruct(Projectile destroyedBy)
    {
        OnDestroyEvent?.Invoke(this, destroyedBy);
        HandleDestruction();
        DisableObjectInternally();
    }

    protected abstract void HandleDestruction();

    protected void DisableObjectInternally()
    {
        // TODO: Pooling.
        Destroy(gameObject, 1f);
    }
}
