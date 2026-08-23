using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    [field: SerializeField] public HitPointComponent HP { get; private set; } 
    [field: SerializeField] public BoxCollider2D Collider { get; protected set; }
    public delegate void DestructionEventHandler(DestructibleObject destroyed, Projectile by); 
    public event DestructionEventHandler OnDestroyEvent;

    void Awake()
    {
        // Collider = GetComponent<Collider2D>();
        Collider.isTrigger = true; 
    }

    public void OnProjectileCollision(Projectile projectile) 
    {
        if (!CanCollideWith(projectile))
            return;

        ReceiveHit(projectile);
    }
    
    public virtual bool CanCollideWith(Projectile projectile)
    {
        return HP.CanTakeHit(); 
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
        OnDestroyEvent?.Invoke(this, destroyedBy);
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
