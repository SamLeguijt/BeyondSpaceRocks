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
 
    protected void SetColliderEnabled(bool value)
    {
        Collider.enabled = value;
    }

    public virtual bool CanInteractWith(Projectile projectile)
    {
        return Collider.enabled && HP.CanTakeHit(); 
    }

    public void InteractWith(Projectile projectile) 
    {
        ReceiveHit(projectile);
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
