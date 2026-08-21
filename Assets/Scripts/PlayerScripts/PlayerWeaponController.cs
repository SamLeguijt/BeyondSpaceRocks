using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public ColorData CurrentColor {  get; private set; }
    public bool AllowedToShoot { get; private set; } = false;

    [Header("References")]
    [SerializeField] private WeaponData defaultWeaponData = null;
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private PlayerColorController playerColor = null;
    [SerializeField] private Animator weaponAnimator = null;
    [SerializeField] private Timer cooldownTimer = null;
    [SerializeField] private string animatorShootClipName = "Shoot";

    public AbstractWeapon CurrentWeapon { get; private set; } =  null;

    private void Start()
    {
        if (CurrentWeapon == null)
            EquipDefaultWeapon();

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

    private void OnShootInputReceivedEvent()
    {
        if (!inputController.IsInputActive)
            return;

        ShootBullet();
    }

    private void EquipDefaultWeapon()
    {
        CurrentWeapon = new DefaultWeapon(defaultWeaponData);
    }

    private void ShootBullet()
    {
        if (!CanShoot())
            return;

        if (!CurrentWeapon.HasAmmo())
        {
            ReloadWeapon();
            return; // Needs visual feedback for players. 
        }
        else
        {
            weaponAnimator.Play(animatorShootClipName);
            CurrentWeapon?.Fire(firePoint.position, CurrentColor);
            cooldownTimer.StartTimer(CurrentWeapon.WeaponData.CooldownSeconds);
        }
    }

    private void ReloadWeapon()
    {
        if (cooldownTimer.IsRunning)
            cooldownTimer.Complete();

        cooldownTimer.StartTimer(CurrentWeapon.WeaponData.ReloadSeconds, () => 
        {
            CurrentWeapon.RefillAmmo();
        });
    }

    private bool CanShoot()
    {
        bool value = CurrentWeapon != null && AllowedToShoot && !cooldownTimer.IsRunning;

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
