using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public float CurrentScore { get; private set; }
    public float TotalScore { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreTMP = null;

    private void Start()
    {
        CurrentScore = 0;
        scoreTMP.text = CurrentScore.ToString();
    }

    private void OnEnable()
    {
        Obstacle.DestructObjectEvent += OnObstacleProjectileCollisionEvent;
    }

    private void OnDisable()
    {
        Obstacle.DestructObjectEvent -= OnObstacleProjectileCollisionEvent;
    }

    private void OnObstacleProjectileCollisionEvent(DestructibleObject obstacle, Projectile projectile)
    {
        // CurrentScore += obstacle.ColorData.Score;

        scoreTMP.text = CurrentScore.ToString();
        AudioManager.Instance?.PlayScoreIncreaseSFX();
    }
}
