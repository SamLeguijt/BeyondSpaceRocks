using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMeter : MonoBehaviour
{
    public float CurrentValue { get; private set; }
    public float MaxValue { get; private set; }
    public float IntervalDelaySeconds { get; private set; }
    public float RechargeDuration { get; private set; }
    public float depletionIntervalAmount; 
    public Action comboReachedLimitEvent; 
    public Action comboAdvanceEvent; 
    public Action comboDepleteEvent; 
    public Action comboReachedMinEvent;

    private Timer comboIntervalTimer; 
    private Timer comboCooldownTimer;
    private Coroutine depletionCoroutine = null;
    
    public ComboMeter(ComboData data)
    {
        MaxValue = data.Limit;
        CurrentValue = 0; 
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

        depletionCoroutine = StartCoroutine(DepleteOverTimeRoutine(RechargeDuration, depletionIntervalAmount));
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
