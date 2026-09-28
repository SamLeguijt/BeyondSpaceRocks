using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Projectiles/new ProjectileConfig", fileName = "ProjectileConfig_")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed;
    [SerializeField] private ColorData colorData;
    [SerializeField] private EProjectileCollisionResponse collisionResponse;
    [SerializeField] private EInteractionRule interactionRule;

    public ProjectileData CreateRuntimeData()
    {
        return new ProjectileData(prefab, speed, collisionResponse, interactionRule);
    }
}