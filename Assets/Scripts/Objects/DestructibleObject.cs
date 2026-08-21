using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IProjectileTarget
{
    
    public int HitPoints { get; protected set; }
    
    public virtual void OnCollisionEnter2D(Collision2D collision) 
    {
        
    }

    public virtual void OnProjectileCollision(Projectile projectile) 
    {

    }
}
