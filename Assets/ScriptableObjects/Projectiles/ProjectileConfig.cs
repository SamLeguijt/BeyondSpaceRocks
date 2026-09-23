using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectiles/new ProjectileConfig", fileName = "ProjectileConfig_")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed;
    [SerializeField] private ColorData colorData;
    [SerializeField] private EProjectileCollisionResponse collisionResponse;
    [SerializeField] private EInteractionRule interactionRule;
    [SerializeField] private ProjectileData data = null;

    public ProjectileData CreateRuntimeData()
    {
        if (data != null)
            return data;

        data = new ProjectileData(prefab, speed, collisionResponse, interactionRule);
        return data;
    }
}