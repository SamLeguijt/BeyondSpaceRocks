using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public ProjectileData ConfigData { get; private set; }
    public ColorData ColorData { get; protected set; }
    public Action<BaseProjectile, Collider2D> ProjectileCollisionEvent; 

    [Header("Component References")]
    [SerializeField] protected SpriteRenderer spriteRenderer = null;
    [SerializeField] protected MovementBehaviour Movement = null;

    public  void Instantiate(ProjectileData data)
    {
        Configure(data);
        Movement.SetActive(true);
        Movement.SetDirection(new Vector2(0, -1));
    }

    protected virtual void Configure(ProjectileData data)
    {
        ConfigData = data; 
        Movement.SetSpeed(data.Speed);
        ColorData = data.ColorData;
        spriteRenderer.color = ColorData.Color;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Movement.IsActive)
            return;

        ProjectileCollisionEvent?.Invoke(this, collision);
    }
}
