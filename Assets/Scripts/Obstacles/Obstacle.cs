using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class Obstacle : DestructibleObject, IScoreSource
{
    public static Action<Obstacle> ObstacleEscapedEvent; 
    public ColorData ColorData { get; protected set; }
    public float ScoreValue => ColorData.Score;

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
        SetupVisuals();
    }
    
    private void SetupVisuals()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (obstacleSprites != null && obstacleSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, obstacleSprites.Length);
            spriteRenderer.sprite = obstacleSprites[randomIndex];
        }

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

        Collider.enabled = false;
        FallBehaviour.SetActive(true);
    }

    public override bool CanReceiveHit(Projectile projectile)
    {
        return base.CanReceiveHit(projectile) 
                && ColorData.ColorType == projectile.ColorData.ColorType;
    }

    protected override void HandleDestruction()
    {
        hitEffectAnimator.Play(hitAnimations[randomHitEffectAnimation].name);
        hitMaskAnimator.Play(hitAnimationMasks[randomHitEffectAnimation].name);

        FallBehaviour.SetActive(false);
        spriteRenderer.sprite = null;
    }

    protected virtual void EnterPlayfield()
    {
        SetColliderEnabled(true);
    }

    protected virtual void EscapePlayfield()
    {
        SetColliderEnabled(false);

        AudioManager.Instance?.PlayObjectEscapedSFX();
        ObstacleEscapedEvent?.Invoke(this);
       
        DestructObject();
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
            EnterPlayfield();

        if (!isInPlayfield && wasInsidePlayfield)
            EscapePlayfield();

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
