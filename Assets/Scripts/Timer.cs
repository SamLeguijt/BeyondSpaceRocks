using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour 
{
    public float Duration {  get; private set; }
    public float Remaining {  get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsFinished { get; private set; }

    private float currentTime; 

    public void StartTimer(float durationSeconds)
    {
        Reset();
        
        Duration = durationSeconds;
        Remaining = Duration; 
        IsFinished = false;
        IsRunning = true; 
    }

    public void Reset()
    {
        IsRunning = false;
        IsFinished = false;
        Duration = 0;
        Remaining = 0; 
    }

    private void Update()
    {
        if (IsRunning && !IsFinished)
            Tick();
    }

    public void Tick()
    {
        if (Remaining <= 0)
        {
            Complete();
        }
        else
        {
            Remaining -= Time.deltaTime;
        }
    }

    public void Complete()
    {
        IsRunning = false; 
        IsFinished = true;
    }
}
