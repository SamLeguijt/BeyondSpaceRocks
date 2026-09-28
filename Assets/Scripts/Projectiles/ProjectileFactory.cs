using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileFactory 
{
    private Dictionary<GameObject, ObjectPool<BaseProjectile>> prefabPools = new();
    private Dictionary<BaseProjectile, ObjectPool<BaseProjectile>> projectilePools = new();

    private ObjectPool<BaseProjectile> GetPool(GameObject prefab)
    {
        if (prefabPools.ContainsKey(prefab))
            return prefabPools[prefab];
        else
        {
            ObjectPool<BaseProjectile> pool = new(
                () => CreateBase(prefab),
                null,
                OnPoolRelease,
                OnPoolDestroy
            );

            prefabPools[prefab] = pool;
            return pool;
        }
    }

    private ObjectPool<BaseProjectile> GetPool(BaseProjectile projectile)
    {
        if (projectilePools.ContainsKey(projectile))
            return projectilePools[projectile];
        else
            throw new System.Exception("[ProjectileFactory] No pool found for projectile");
    }

    public BaseProjectile Get(ProjectileData data)
    {
        var pool = GetPool(data.Prefab);
        BaseProjectile projectile = pool.Get();

        projectilePools[projectile] = pool;

        projectile.Configure(data);
        return projectile;
    }

    public void Return(BaseProjectile projectile)
    {
        var pool = GetPool(projectile);
        pool.Release(projectile);

        projectilePools.Remove(projectile);
    }

    private BaseProjectile CreateBase(GameObject prefab)
    {
        if (!prefab.GetComponent<BaseProjectile>())
            throw new System.Exception("[ProjectileFactory] Cannot use a Prefab without BaseProjectile component"); 

        GameObject instantiated = GameObject.Instantiate(prefab);
        return instantiated.GetComponent<BaseProjectile>();
    }

    private void OnPoolRelease(BaseProjectile projectile)
    {
        projectile.Reset();
    }

    private void OnPoolDestroy(BaseProjectile projectile)
    {
        GameObject.Destroy(projectile.gameObject);
    }
}
