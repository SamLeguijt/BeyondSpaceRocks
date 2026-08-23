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
        DestructibleObject.ObjectDestroyEvent += OnObjectDestroyed;
    }

    private void OnDisable()
    {
        DestructibleObject.ObjectDestroyEvent -= OnObjectDestroyed;
    }

    private void OnObjectDestroyed(DestructibleObject obstacle, Projectile projectile)
    {
        IScoreSource source = obstacle as IScoreSource; 

        if (source != null)
        {
            CurrentScore += source.ScoreValue;  
            OnScoreChanged();
        }
    }

    private void OnScoreChanged()
    {
        scoreTMP.text = CurrentScore.ToString();
        AudioManager.Instance?.PlayScoreIncreaseSFX();
    }
}
