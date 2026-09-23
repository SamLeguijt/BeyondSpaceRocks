using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData 
{
    public GameObject Prefab { get; protected set; }
    public ColorData ColorData { get; set; } 
    public EProjectileCollisionResponse CollisionResponse { get; protected set; } 
    public EInteractionRule InteractionRule {get; protected set; }
    
    public Vector2 SpawnPosition { get; set; }
    public Vector2 MoveDirection { get; set; }
    public float MoveSpeed { get; protected set; }


    public void ApplyModifier()
    {
        // todo: route modifiers 
    }

    public ProjectileData(
        GameObject prefab, 
        float speed, 
        EProjectileCollisionResponse collisionResponse,
        EInteractionRule interactionRule,
        ColorData colorData = new(), 
        Vector2 spawnPos = new Vector2(), 
        Vector2 moveDir = new Vector2()
        )
    {
        Prefab = prefab;
        MoveSpeed = speed;
        CollisionResponse = collisionResponse;
        InteractionRule = interactionRule;
        ColorData = colorData;
        SpawnPosition = spawnPos;
        MoveDirection = moveDir;
    }
}
