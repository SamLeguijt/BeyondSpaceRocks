using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : DestructibleObject
{
    public delegate void ObstcaleCollisionHandler(Obstacle obstacle, Projectile projectile);
    public delegate void ObstacleEscapedHandler(Obstacle obstacle);
    public static event ObstcaleCollisionHandler ObstacleProjectileCollisionEvent;
    public static event ObstacleEscapedHandler ObstacleEscapedEvent;

    public ColorData ColorData { get; protected set; }
    [SerializeField] private Sprite[] obstacleSprites = null;

    [field: SerializeField] public FallingBehaviour FallBehaviour { get; private set; }

    [Header("Effect References")]
    [SerializeField] private Animator hitEffectAnimator = null;
    [SerializeField] private Animator hitMaskAnimator = null;
    [SerializeField] private SpriteRenderer maskHitRenderer = null;
    [SerializeField] private List<AnimationClip> hitAnimations = null;
    [SerializeField] private List<AnimationClip> hitAnimationMasks = null;
    private SpriteRenderer spriteRenderer;
    private int randomHitEffectAnimation = 0;
    private Bounds playFieldBounds = default;
    private bool wasInsidePlayfield = false;

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
        FallBehaviour.SetActive(false);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (obstacleSprites != null && obstacleSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, obstacleSprites.Length);
            spriteRenderer.sprite = obstacleSprites[randomIndex];
        }

        Collider.enabled = false;
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

        FallBehaviour.SetFallSpeed(Random.Range(ColorData.MinSpeed, ColorData.MaxSpeed));

        spriteRenderer.color = this.ColorData.Color;
        maskHitRenderer.color = this.ColorData.Color;

        FallBehaviour.SetActive(true);
    }

    public override bool CanReceiveHit(Projectile projectile)
    {
        return base.CanReceiveHit(projectile) && ColorData.ColorType == projectile.ColorData.ColorType;
    }

    protected override void OnDestruct()
    {
        hitEffectAnimator.Play(hitAnimations[randomHitEffectAnimation].name);
        hitMaskAnimator.Play(hitAnimationMasks[randomHitEffectAnimation].name);

        FallBehaviour.SetActive(false);
        spriteRenderer.sprite = null;
    }

    protected virtual void EnterPlay()
    {
        // Enable collision/hit taken 
        Collider.enabled = true; 
    }

    protected virtual void EscapePlay()
    {
        AudioManager.Instance?.PlayObjectEscapedSFX();
        ObstacleEscapedEvent?.Invoke(this);
        FallBehaviour.SetActive(false);
        OnDestruct();
        Destroy(gameObject); // <- Pool
    }

    void Update()
    {
        if (FallBehaviour.IsActive)
            CheckPlayfieldChanges();
    }

    private void CheckPlayfieldChanges()
    {
        bool isInPlayfield = IsInsidePlayField();

        if (isInPlayfield && !wasInsidePlayfield)
            EnterPlay();

        if (!isInPlayfield && wasInsidePlayfield)
            EscapePlay();

        wasInsidePlayfield = isInPlayfield;
    }

    private bool IsInsidePlayField()
    {
        if (playFieldBounds == default) 
            playFieldBounds = GameManager.Instance.PlayFieldBounds;
        
        float x = transform.position.x;
        float y = transform.position.y;

        Vector2 spriteSizeOffset = spriteRenderer.sprite.bounds.size;

        float fieldMinX = playFieldBounds.min.x + spriteSizeOffset.x;
        float fieldMaxX = playFieldBounds.max.x - spriteSizeOffset.x; 
        float fieldMinY = playFieldBounds.min.y - spriteSizeOffset.y;
        float fieldMaxY = playFieldBounds.max.y + spriteSizeOffset.y; 

        return x > fieldMinX && x < fieldMaxX &&
                y > fieldMinY && y < fieldMaxY;  
    }
}
