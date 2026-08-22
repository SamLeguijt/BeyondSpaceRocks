using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public delegate void PlayerHealthHandler();

    public event PlayerHealthHandler PlayerGameOverEvent;
    public event PlayerHealthHandler PlayerLoseLifeEvent;

    public float MaxLives => maxLives; 
    public float CurrentLives => currentLives;
    public bool IsAlive => currentLives > 0;

    [Header("Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int currentLives = 0;

    [Header("Debug tools")]
    [SerializeField] private bool takeDamage = false;

    private void Start()
    {
        currentLives = maxLives;
    }

    private void OnEnable()
    {
        Obstacle.ObstacleEscapedEvent += OnObstacleEscapedEvent;
    }

    private void OnDisable()
    {
        Obstacle.ObstacleEscapedEvent -= OnObstacleEscapedEvent;

    }

    private void Update()
    {
        // DEBUG.
        if (takeDamage)
        {
            LoseLives(1);
            takeDamage = false;
        }
    }

    private void OnObstacleEscapedEvent(Obstacle obstacle)
    {
        LoseLives(1);
    }

    public void LoseLives(int amount)
    {
        currentLives -= amount;

        PlayerLoseLifeEvent?.Invoke();

        if (currentLives <= 0)
            GameOver();
    }

    private void GameOver()
    {
        PlayerGameOverEvent?.Invoke();
    }
}
