using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMeter : MonoBehaviour
{
    public ComboData data; 
    public float CurrentValue { get; private set; }
    public float MaxValue { get; private set; }
    public float IntervalDelaySeconds { get; private set; }
    public float RechargeDuration { get; private set; }
    public float DepletionIntervalAmount { get; private set; } 
    
    public Action comboReachedLimitEvent; 
    public Action comboAdvanceEvent; 
    public Action comboDepleteEvent; 
    public Action comboReachedMinEvent;

    private Timer comboIntervalTimer; 
    private Timer comboCooldownTimer;
    private Coroutine depletionCoroutine = null;

    void Awake()
    {
        if (data != null)
        {
            MaxValue = data.Limit;
            IntervalDelaySeconds = data.comboIntervalSeconds;
            RechargeDuration = data.comboRechargeCooldownSeconds;
            DepletionIntervalAmount = 1;
        }
        
        CurrentValue = 0;
    }

    public ComboMeter(ComboData data)
    {
        MaxValue = data.Limit;
        CurrentValue = 0; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Advance(1);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Deplete(1);
        }

        float lastFrameValue = CurrentValue; 

        if (CurrentValue != lastFrameValue)
        {
            Debug.Log(CurrentValue);
        }     
    }

    public bool HasReachedMax()
    {
        return CurrentValue >= MaxValue; 
    }

    public bool CanAdvance()
    {
        bool cooldownCheck = !comboCooldownTimer.IsRunning && comboIntervalTimer.IsRunning;
        bool limitCheck =  CurrentValue < MaxValue;

        return cooldownCheck && limitCheck;
    }

    private bool CanDeplete()
    {
        bool cooldownCheck = comboCooldownTimer.IsRunning;
        bool valueCheck = CurrentValue > 0; 

        return cooldownCheck && valueCheck;
    }

    public void Advance(float amount)
    {
        if (CanAdvance())
        {
            CurrentValue = Mathf.Min(CurrentValue + amount, MaxValue);
            comboAdvanceEvent?.Invoke();

            comboIntervalTimer.StartTimer(IntervalDelaySeconds, StartDepletion);
        }
        else if (HasReachedMax())
        {
            LimitReached();
        }
    }

    public void Deplete(float amount)
    {
        if (CanDeplete())
        {
            CurrentValue = Mathf.Max(CurrentValue - amount, 0);
            comboDepleteEvent?.Invoke();
        }
    }

    private void LimitReached()
    {
        comboReachedLimitEvent?.Invoke();
    }

    private void StartDepletion()
    {
        if (depletionCoroutine != null)
        {
            StopCoroutine(depletionCoroutine);
        }

        depletionCoroutine = StartCoroutine(DepleteOverTimeRoutine(RechargeDuration, DepletionIntervalAmount));
    }

    private IEnumerator DepleteOverTimeRoutine(float maxDuration, float depleteAmountPerInterval)
    {
        float depletionInterval = CurrentValue / maxDuration;

        WaitForSeconds intervalDelay = new WaitForSeconds(depletionInterval);

        while (CanDeplete())
        {
            yield return intervalDelay;

            Deplete(depleteAmountPerInterval);
        }
    }
}
