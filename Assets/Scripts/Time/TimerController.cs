using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static List<Timer> timers = new();

    void Awake()
    {
        if (timers != null)
            timers.Clear();

        timers = new List<Timer>();
    }

    public static void Subscribe(Timer timer)
    {
        if (!timers.Contains(timer))
            timers.Add(timer);

        Debug.Log("Subscribed");
    }

    public static void Unsubscribe(Timer timer)
    {
        if (timers.Contains(timer))
            timers.Remove(timer);
    }

    void Update()
    {
        int timersAmount = timers.Count;

        if (timersAmount > 0)
        {
            for (int i = 0; i < timersAmount; i++)
            {
                timers[i].Tick(Time.deltaTime);
            }
        }
    }
}
