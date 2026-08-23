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
    protected float playFieldBoundsY;

    public virtual void Instantiate(float speed, ColorData color)
    {
        playFieldBoundsY = GameManager.Instance.PlayFieldBounds.max.y;
        
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

        if (transform.position.y > playFieldBoundsY)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnabled)
            return;

        IProjectileTarget target = collision.GetComponent<IProjectileTarget>();

        if (target == null)
            return;

        if (target.CanInteractWith(this))
        {
            target.InteractWith(this);
            OnCollision();
        }
    }

    protected virtual void OnCollision()
    {
        HasCollided = true;
        Destroy(gameObject);
    }
}
