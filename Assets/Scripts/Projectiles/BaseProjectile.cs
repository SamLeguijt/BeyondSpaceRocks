using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public ProjectileData Data { get; private set; }
    public Action<BaseProjectile, Collider2D> ProjectileCollisionEvent; 

    [Header("Component References")]
    [SerializeField] protected SpriteRenderer spriteRenderer = null;
    [SerializeField] protected MovementBehaviour Movement = null;

    public virtual void Configure(ProjectileData data)
    {
        Data = data; 
        Movement.SetSpeed(data.MoveSpeed);
        spriteRenderer.color = Data.ColorData.Color;
    }

    public virtual void Activate()
    {
        if (Data == null)
            return;

        Movement.SetSpeed(Data.MoveSpeed);
        Movement.SetDirection(Data.MoveDirection);
        Movement.SetActive(true);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Movement.IsActive)
            return;

        ProjectileCollisionEvent?.Invoke(this, collision);
    }
}
