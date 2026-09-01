using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMeter : MonoBehaviour
{
    [field: SerializeField] public float MinValue { get; private set; } = 0;
    [field: SerializeField] public float MaxValue { get; private set; } = 0;

    public float CurrentValue { get; private set; } = 0;
    public bool IsEmpty => CurrentValue <= MinValue;
    public bool IsFull => CurrentValue >= MaxValue;

    public Action<float> valueChangedEvent; 

    private Coroutine depletionCoroutine = null;

    void Awake()
    {
        CurrentValue = MinValue;
    }

    void Update()
    {   
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     Advance(1);
        // }
        // else if (Input.GetKeyDown(KeyCode.P))
        // {
        //     Deplete(1);
        // }
        // else if (Input.GetKeyDown(KeyCode.U))
        // {
        //     DepleteOverTime(3);
        // }
        // else if (Input.GetKeyDown(KeyCode.I))
        // {
        //     DepleteInTicks(0.5f, 5);
        // }
    }

    public void Advance(float amount)
    {
        if (amount <= 0)
            return;

        float newValue = Mathf.Min(CurrentValue + amount, MaxValue);
        float changedAmount = newValue - CurrentValue;

        CurrentValue = newValue;

        if (!Mathf.Approximately(changedAmount, 0f))
        {
            valueChangedEvent?.Invoke(changedAmount);
            
            // Debug.Log("Advanced to: " + CurrentValue);
        }
    }

    public void Deplete(float amount)
    {
        if (amount <= 0)
            return;

        float newValue = Mathf.Max(CurrentValue - amount, MinValue);
        float changedAmount = newValue - CurrentValue;

        CurrentValue = newValue;

        if (!Mathf.Approximately(changedAmount, 0f))
        {
            valueChangedEvent?.Invoke(changedAmount);
            
            // Debug.Log("Depleted to: " + CurrentValue);
        }
    }

    public void DepleteOverTime(float seconds)
    {
        if (depletionCoroutine != null)
        {
            StopCoroutine(depletionCoroutine);
        }

        depletionCoroutine = StartCoroutine(DepleteOverTimeRoutine(seconds));
        Debug.Log("Start DOT");
    }

    public void DepleteInTicks(float amountPerTick, float maxDuration)
    {
        if (depletionCoroutine != null)
        {
            StopCoroutine(depletionCoroutine);
        }

        depletionCoroutine = StartCoroutine(DepleteTicksOverTimeRoutine(amountPerTick, maxDuration));
    }

    private IEnumerator DepleteTicksOverTimeRoutine(float amountPerTick, float maxDuration)
    {
        if (maxDuration <= 0)
        {
            Deplete(CurrentValue - MinValue);
            depletionCoroutine = null;
            yield break;
        }

        float amountToDeplete = CurrentValue - MinValue;

        if (amountToDeplete <= 0)
        {
            depletionCoroutine = null;
            yield break;
        }
        
        if (amountPerTick <= 0)
        amountPerTick = 0.01f;
    
        int amountOfTicks = Mathf.CeilToInt((CurrentValue - MinValue) / amountPerTick);
        float tickInterval = maxDuration / amountOfTicks;

        WaitForSeconds tickDelay = new WaitForSeconds(tickInterval);

        for (int i = 0; i < amountOfTicks; i++)
        {
            yield return tickDelay;

            Deplete(amountPerTick);
        }

        Deplete(CurrentValue - MinValue);
        depletionCoroutine = null;
    }

    private IEnumerator DepleteOverTimeRoutine(float maxDuration)
    {
        if (maxDuration <= 0)
        {
            Deplete(CurrentValue - MinValue);
            depletionCoroutine = null;
            yield break;
        }

        float startValue = CurrentValue;
        float elapsed = 0f;
        float maxSeconds = maxDuration;

        while (elapsed < maxSeconds)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / maxSeconds);
            float targetValue = Mathf.Lerp(startValue, MinValue, t);
            float amountToDeplete = CurrentValue - targetValue;
            
            Deplete(amountToDeplete);

            yield return null;
        }

        Deplete(CurrentValue - MinValue);
        depletionCoroutine = null;
    }
}
