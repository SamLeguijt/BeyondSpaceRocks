using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool HasCollided {  get; protected set; }
    public ColorData ColorData { get; protected set; }

    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer = null;

    protected float speed = 0f;
    protected bool isEnabled = false;
    protected Vector2 screenBounds;

    protected int projectileTargetCollisionLayer { get; private set; } 

    public virtual void Instantiate(float speed, ColorData color)
    {
        screenBounds = GameManager.Instance.GetScreenBounds();
        projectileTargetCollisionLayer = GameManager.Instance.ProjectileTargetCollisionLayer; 
        
        this.speed = speed;
        ColorData = color;
        spriteRenderer.color = ColorData.Color;

        isEnabled = true;
    }

    protected virtual void Update()
    {
        if (!isEnabled)
            return;

        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);

        if (transform.position.y > screenBounds.y)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnabled)
            return;

        if (collision.gameObject.layer != projectileTargetCollisionLayer)
            return; 

        IProjectileTarget target = collision.GetComponent<IProjectileTarget>();

        if (target == null)
            return;

        if (target.CanCollideWith(this))
        {
            target.OnProjectileCollision(this);
            OnCollision();
        }
    }

    protected virtual void OnCollision()
    {
        HasCollided = true;
        Destroy(gameObject);
    }
}
