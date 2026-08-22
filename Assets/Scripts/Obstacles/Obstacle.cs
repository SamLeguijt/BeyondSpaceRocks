using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public delegate void ObstcaleCollisionHandler(Obstacle obstacle, Projectile projectile);
    public delegate void ObstacleEscapedHandler(Obstacle obstacle);

    public static event ObstcaleCollisionHandler ObstacleProjectileCollisionEvent;
    public static event ObstacleEscapedHandler ObstacleEscapedEvent;

    public static float BottomBorder = 2.2f;

    public ColorData ColorData { get; private set; }
    [SerializeField] private Sprite[] obstacleSprites = null;

    [Header("Effect References")]
    [SerializeField] private Animator hitEffectAnimator = null;
    [SerializeField] private Animator hitMaskAnimator = null;
    [SerializeField] private SpriteRenderer maskHitRenderer = null;
    [SerializeField] private List<AnimationClip> hitAnimations = null;
    [SerializeField] private List<AnimationClip> hitAnimationMasks = null;

    private Collider2D obstacleCollider = null;
    private bool canBeDestroyed = false;
    private bool isActive = false;
    private float fallSpeed = 1f;
    private int projectileLayer;
    private Vector2 screenBounds;

    private SpriteRenderer spriteRenderer;
    private float spriteSizeHalfedY;

    private int randomHitEffectAnimation = 0;

    private void OnEnable()
    {
        GameManager.Instance.GameEndedEvent += OnGameEndedEvent;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameEndedEvent -= OnGameEndedEvent;
    }

    private void OnGameEndedEvent()
    {
        isActive = false;
    }

    private void Awake()
    {
        obstacleCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSizeHalfedY = spriteRenderer.sprite.bounds.size.y / 2;

        // if (obstacleSprites != null && obstacleSprites.Length > 0)
        // {
        //     int randomIndex = Random.Range(0, obstacleSprites.Length);
        //     spriteRenderer.sprite = obstacleSprites[randomIndex];
        // }

        obstacleCollider.isTrigger = true;

        screenBounds = GameManager.Instance.GetScreenBounds();
        projectileLayer = GameManager.Instance.ProjectileLayerIndex;
    }

    private void Start()
    {
        randomHitEffectAnimation = Random.Range(0, hitAnimations.Count);

        float zRot = Random.Range(0f, 360f);
        hitEffectAnimator.gameObject.transform.rotation = Quaternion.Euler(0, 0, zRot);
    }

    public void Initialize(ColorData colorData)
    {
        this.ColorData = colorData;
        this.fallSpeed = Random.Range(ColorData.MinSpeed, ColorData.MaxSpeed);

        spriteRenderer.color = this.ColorData.Color;
        maskHitRenderer.color = this.ColorData.Color;

        isActive = true;
    }

    protected virtual void OnProjectileCollision(Projectile projectile)
    {
        if (!canBeDestroyed)
            return; 

        if (projectile.ColorData.ColorType != ColorData.ColorType)
            return;

        ObstacleProjectileCollisionEvent?.Invoke(this, projectile);
        DisableObstacleOnDeath();
    }

    private void DisableObstacleOnDeath()
    {
        hitEffectAnimator.Play(hitAnimations[randomHitEffectAnimation].name);
        hitMaskAnimator.Play(hitAnimationMasks[randomHitEffectAnimation].name);

        isActive = false;

        obstacleCollider.enabled = false;
        canBeDestroyed = true;

        spriteRenderer.sprite = null;
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (!canBeDestroyed)
        {
            if (transform.position.y < 2.9f)//(screenBounds.y - spriteSizeHalfedY * 2 ))
                canBeDestroyed = true;
        }

        transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed * GameManager.Instance.SimulationSpeed * Time.deltaTime);

        if (transform.position.y < (-BottomBorder - spriteSizeHalfedY))
        {
            AudioManager.Instance?.PlayObjectEscapedSFX();
            ObstacleEscapedEvent?.Invoke(this);
            isActive = false;
            Destroy(gameObject);
        }
    }
}
