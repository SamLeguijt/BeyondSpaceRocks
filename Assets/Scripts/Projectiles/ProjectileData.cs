using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectiles/new ProjectileData", fileName = "ProjectileData_")]
public class ProjectileData : ScriptableObject
{
    // TODO: Private set + ApplyModifier methods?

    public GameObject Prefab { get; set; }
    public float Speed { get; set; } 
    public ColorData ColorData { get; set; } 
    public EProjectileCollisionResponse CollisionResponse { get; set; } 
    public EInteractionRule InteractionRule {get; set; }
}
