using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float ProjectileSpeed => projectileSpeed;

    public ColorData CurrentColor {  get; private set; }
    public bool AllowedToShoot { get; private set; } = false;

    [Header("References")]
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private Projectile projectilePRefab = null;
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private PlayerColorController playerColor = null;
    [SerializeField] private Animator weaponAnimator = null;
    [SerializeField] private Timer cooldownTimer = null;
    [SerializeField] private string animatorShootClipName = "Shoot";

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 1f;
    [field: SerializeField] public float ShootCooldownTime { get; private set;  } = 0f;


    private void Start()
    {
        inputController.ShootInput += OnShootInputReceivedEvent;
        playerColor.OnColorChanged += OnColorChangedEvent;
        GameManager.Instance.GameStartedEvent += OnGameStartEvent;
        GameManager.Instance.GameEndedEvent += OnGameEndEvent;
    }

    private void OnDisable()
    {
        inputController.ShootInput -= OnShootInputReceivedEvent;
        GameManager.Instance.GameStartedEvent -= OnGameStartEvent;
        GameManager.Instance.GameEndedEvent -= OnGameEndEvent;
    }

    private void Update()
    {
        
    }

    private void OnShootInputReceivedEvent()
    {
        if (!inputController.IsInputActive)
            return;

        ShootBullet();
    }

    private void ShootBullet()
    {
        if (!CanShoot())
            return;

        AudioManager.Instance?.PlayShootSFX();
        weaponAnimator.Play(animatorShootClipName);
        Projectile bullet = Instantiate(projectilePRefab, firePoint.position, Quaternion.identity);
        bullet.Instantiate(this);
        cooldownTimer.StartTimer(ShootCooldownTime);
    }

    private bool CanShoot()
    {
        bool value = AllowedToShoot && !cooldownTimer.IsRunning;

        return value;
    }

    private void OnColorChangedEvent(ColorData colorData)
    {
        CurrentColor = colorData;
    }

    private void OnGameStartEvent()
    {
        AllowedToShoot = true;
    }

    private void OnGameEndEvent()
    {
        AllowedToShoot = false;
    }
}
