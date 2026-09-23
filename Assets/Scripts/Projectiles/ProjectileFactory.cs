using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// TODO: Objectpooling, prefab through constructor?
public class ProjectileFactory 
{
    public BaseProjectile Create(ProjectileData data, Vector2 position)
    {
        BaseProjectile projectile = GameObject.Instantiate(data.Prefab, position, quaternion.identity).GetComponent<BaseProjectile>();
        projectile.Instantiate(data);

        return projectile;
    }

    public void Return(BaseProjectile projectile)
    {
        // Pooling
    }
}
