using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static List<Timer> timers = new();

    public static void Subscribe(Timer timer)
    {
        if (!timers.Contains(timer))
            timers.Add(timer);
    }

    public static void Unsubscribe(Timer timer)
    {
        if (timers.Contains(timer))
            timers.Remove(timer);
    }

    void Update()
    {
        int timersAmount = timers.Count;
        // Debug.Log("Timers: " + timersAmount);
        if (timersAmount > 1)
        {
            for (int i = 0; i < timersAmount; i++)
            {
                timers[i].Tick(Time.deltaTime);
            }
        }
    }
}
