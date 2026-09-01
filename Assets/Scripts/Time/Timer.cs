using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Timer  
{
    public float Duration {  get; private set; }
    public float Remaining {  get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsFinished { get; private set; }

    private Action onCompletionCallback = null; 
    private bool destructOnCompletion = false;

    public Timer(bool isPersistant)
    {
        TimerController.Subscribe(this);
        destructOnCompletion = !isPersistant;
    }

    public void StartTimer(float durationSeconds, Action callback = null)
    {
        Reset();
        
        Duration = durationSeconds;
        Remaining = Duration; 
        IsFinished = false;
        IsRunning = true; 
        onCompletionCallback = callback; 
    }

    public void Reset()
    {
        IsRunning = false;
        IsFinished = false;
        Duration = 0;
        Remaining = 0; 
        onCompletionCallback = null;
    }

    public void Tick(float deltaTime)
    {
        if (!IsRunning || IsFinished)
            return;

        if (Remaining <= 0)
        {
            Complete();
        }
        else
        {
            Remaining -= deltaTime;
        }
    }

    public void Complete()
    {
        IsRunning = false; 
        IsFinished = true;       
        onCompletionCallback?.Invoke(); 

        if (destructOnCompletion)
            Destruct();
    }

    private void Destruct()
    {
        TimerController.Unsubscribe(this);
    }
}
