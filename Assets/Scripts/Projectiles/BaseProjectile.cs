using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public ProjectileData Data { get; private set; }
    public Action<BaseProjectile, Collider2D> ProjectileCollisionEvent; 

    [field: Header("Component References")]
    [field: SerializeField] public MovementBehaviour Movement { get; protected set; } = null;
    [field: SerializeField] protected SpriteRenderer spriteRenderer { get; private set;  } = null;

    public virtual void Configure(ProjectileData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        Data = data; 
        Movement.SetSpeed(data.MoveSpeed);
        Movement.SetDirection(Data.MoveDirection);
        spriteRenderer.color = Data.ColorData.Color;

        transform.position = Data.SpawnPosition;
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        Movement.SetActive(true);
    }

    public virtual void Reset()
    {
        Movement.Reset();
        Data = null;
        spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Movement.IsActive)
            return;

        ProjectileCollisionEvent?.Invoke(this, collision);
    }
}
