using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    [field: SerializeField] public HitPointComponent HP { get; private set; } 
    [field: SerializeField] public BoxCollider2D Collider { get; protected set; }
    public delegate void DestructionEventHandler(DestructibleObject destroyed, Projectile by); 
    public static event DestructionEventHandler DestructObjectEvent;

    void Awake()
    {
        Collider.isTrigger = true; 
    }

    public void OnProjectileCollision(Projectile projectile) 
    {
        if (!CanReceiveHit(projectile))
            return;

        ReceiveHit(projectile);
    }
    
    public virtual bool CanReceiveHit(Projectile projectile)
    {
        return Collider.enabled && HP.CanTakeHit(); 
    }

    protected virtual void ReceiveHit(Projectile projectile)
    {
        HP.TakeHitPoints(1);

        if (!HP.IsAlive)
        {
            Destruct(projectile);
        }
    }

    protected virtual void Destruct(Projectile destroyedBy)
    {
        Collider.enabled = false; 
        DestructObjectEvent?.Invoke(this, destroyedBy);
        OnDestruct();
        DisableObjectInternally();
    }

    protected abstract void OnDestruct();

    protected void DisableObjectInternally()
    {
        // TODO: Pooling.
        Destroy(gameObject, 1f);
    }
}
