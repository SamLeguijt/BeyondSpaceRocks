using System;
using System.Collections.Generic;

// Source: https://medium.com/@mokarchi/building-a-custom-event-bus-in-net-24b9e195c57d

public class EventBus : IEventBus
{
    private Dictionary<Type, List<Delegate>> handlers = new();
    
    public void Publish<T>(T @event) where T : IEvent
    {
        var eventType = typeof(T);

        if (handlers.ContainsKey(eventType))
        {
            foreach (var handler in handlers[eventType])
            {
                ((Action<T>)handler)(@event);
            }
        }
    }

    public void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        var eventType = typeof(T);

        if (!handlers.ContainsKey(eventType))
        {
            handlers[eventType] = new List<Delegate>();
        }

        handlers[eventType].Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        var eventType = typeof(T);

        if (!handlers.ContainsKey(eventType))
            return;

        handlers[eventType].Remove(handler);
    }
}
