using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : HitPointComponent
{
    public delegate void PlayerHealthHandler();
    public event PlayerHealthHandler PlayerGameOverEvent;
    public event PlayerHealthHandler PlayerLoseLifeEvent;


    [Header("Settings")]

    [Header("Debug tools")]
    [SerializeField] private bool takeDamage = false;

    private void OnEnable()
    {
        Obstacle.ObstacleEscapedEvent += OnObstacleEscapedEvent;
    }

    private void OnDisable()
    {
        Obstacle.ObstacleEscapedEvent -= OnObstacleEscapedEvent;
    }

    public override void TakeHitPoints(int hits)
    {
        base.TakeHitPoints(hits);
        PlayerLoseLifeEvent?.Invoke(); 
    }

    protected override void OnDeath(int lastHit)
    {
        base.OnDeath(lastHit);
        PlayerGameOverEvent?.Invoke();
    }

    private void Update()
    {
        // DEBUG.
        if (takeDamage)
        {
            TakeHitPoints(1);
            takeDamage = false;
        }
    }

    private void OnObstacleEscapedEvent(Obstacle obstacle)
    {
        TakeHitPoints(1);
    }

    private void GameOver()
    {
        PlayerGameOverEvent?.Invoke();
    }
}
