using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool HasCollided {  get; private set; }
    public ColorData CurrentColor { get; private set; }

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private float speed = 0f;
    private bool isEnabled = false;
    private Vector2 screenBounds;

    public void Instantiate(float speed, ColorData color)
    {
        screenBounds = GameManager.Instance.GetScreenBounds();

        this.speed = speed;
    
        CurrentColor = color;
        spriteRenderer.color = CurrentColor.Color;

        isEnabled = true;
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);


        if (transform.position.y > screenBounds.y)
        {
            Destroy(gameObject);
        }
    }

    public void OnObstacleCollision(Obstacle obstacle)
    {
        HasCollided = true;
        Destroy(gameObject);
    }
}
