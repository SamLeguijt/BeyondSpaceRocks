using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    [field: SerializeField] public HitPointComponent HP { get; private set; } 
    [field: SerializeField] public BoxCollider2D Collider { get; protected set; }
    public delegate void DestructionEventHandler(DestructibleObject destroyed, Projectile by); 
    public static event DestructionEventHandler ObjectDestroyEvent;

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
    
    protected void SetColliderEnabled(bool value)
    {
        Collider.enabled = value;
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
            DestructObject(projectile);
        }
    }

    protected void DestructObject(Projectile destroyedBy = null)
    {
        SetColliderEnabled(false);
        HandleDestruction();
        ObjectDestroyEvent?.Invoke(this, destroyedBy);
        DisableObjectInternally();
    }

    protected abstract void HandleDestruction();

    protected void DisableObjectInternally()
    {
        // TODO: Pooling.
        Destroy(gameObject, 1f);
    }
}
