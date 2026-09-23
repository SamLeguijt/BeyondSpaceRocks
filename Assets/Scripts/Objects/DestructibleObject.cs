using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    [field: SerializeField] public HitPointComponent HP { get; private set; } 
    [field: SerializeField] public BoxCollider2D Collider { get; protected set; }

    public abstract ColorData ColorData { get; protected set; }

    public delegate void DestructionEventHandler(DestructibleObject destroyed, IProjectile by); 
    public static event DestructionEventHandler ObjectDestroyEvent;

    void Awake()
    {
        Collider.isTrigger = true; 
    }
 
    protected void SetColliderEnabled(bool value)
    {
        Collider.enabled = value;
    }

    public virtual bool CanInteractWith(IProjectile projectile)
    {
        return Collider.enabled && HP.CanTakeHit(); 
    }

    public void InteractWith(IProjectile projectile) 
    {
        ReceiveHit(projectile);
    }

    protected virtual void ReceiveHit(IProjectile projectile)
    {
        HP.TakeHitPoints(1);

        if (!HP.IsAlive)
        {
            DestructObject(projectile);
        }
    }

    protected void DestructObject(IProjectile destroyedBy = null)
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

    public void OnProjectileHit(BaseProjectile projectile)
    {
        throw new System.NotImplementedException();
    }
}
