using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// TODO: Objectpooling
public class ProjectileFactory 
{
    public BaseProjectile Create(ProjectileData data)
    {
        BaseProjectile projectile = GameObject.Instantiate(data.Prefab, data.SpawnPosition, Quaternion.Euler(data.MoveDirection)).GetComponent<BaseProjectile>();

        if (projectile == null)
            throw new System.Exception();

        projectile.Configure(data);
        return projectile;
    }

    public void Return(BaseProjectile projectile)
    {
        GameObject.Destroy(projectile.gameObject);
        // Pooling
    }
}
