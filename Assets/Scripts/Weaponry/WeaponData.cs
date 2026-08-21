using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_", menuName = "ScriptableObjects/Weaponry/new WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    [field: SerializeField] public Projectile ProjectilePrefab { get; protected set; } = null;

    [field: SerializeField] public float ProjectileSpeed { get; protected set; } = 1f;

    [field: SerializeField] public int MaxAmmo { get; protected set; } = 1;

    [field: SerializeField] public float CooldownSeconds { get; protected set; } = 0.5f;

    [field: SerializeField] public float ReloadSeconds { get; protected set; } = 0.0f;
}
