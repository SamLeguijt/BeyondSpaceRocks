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

    public Action<AbstractWeapon> weaponFireEvent;

    public Action<AbstractWeapon, float> weaponReloadStartEvent; 

    public Action<AbstractWeapon> weaponReloadFinishedEvent; 
    public Action<AbstractWeapon> equipWeaponEvent;

    void Awake()
    {
        cooldownTimer = new Timer(isPersistant: true);
    }

    private void Start()
    {
        if (CurrentWeapon == null)
            EquipWeapon(new DefaultWeapon(defaultWeaponData));
    }

    void OnEnable()
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

    private void SetCurrentWeapon(AbstractWeapon weapon)
    {
        if (weapon == null)
            weapon = new DefaultWeapon(defaultWeaponData);

        CurrentWeapon = weapon;
    }

    private void OnShootInputReceivedEvent()
    {
        if (!inputController.IsInputActive)
            return;

        HandleShoot();
    }

    public void EquipWeapon(AbstractWeapon weapon)
    {
        SetCurrentWeapon(weapon);
        equipWeaponEvent?.Invoke(weapon);
    }

    private void HandleShoot()
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
            FireCurrentWeapon();
        }
    }

    private void FireCurrentWeapon()
    {
        weaponAnimator.Play(animatorShootClipName);
        CurrentWeapon?.Fire(firePoint.position, CurrentColor);
        weaponFireEvent?.Invoke(CurrentWeapon);
        cooldownTimer.StartTimer(CurrentWeapon.WeaponData.CooldownSeconds);
    }

    private void ReloadWeapon()
    {
        if (cooldownTimer.IsRunning)
            cooldownTimer.Complete();

        float reloadDuration = CurrentWeapon.WeaponData.ReloadSeconds;

        weaponReloadStartEvent?.Invoke(CurrentWeapon, reloadDuration);
        cooldownTimer.StartTimer(CurrentWeapon.WeaponData.ReloadSeconds, () => 
        {
            CurrentWeapon.RefillAmmo();
            weaponReloadFinishedEvent?.Invoke(CurrentWeapon);
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
