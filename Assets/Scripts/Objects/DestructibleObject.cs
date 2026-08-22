using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    [field: SerializeField] public HitPointComponent HP { get; private set; } 

    public virtual void OnProjectileCollision(Projectile projectile) 
    {
        
    }
}
