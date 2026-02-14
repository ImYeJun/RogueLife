using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<TEventData>(Action<TEventData> action)
    {
        Type eventType = typeof(TEventData);

        if (!subscribers.ContainsKey(eventType))
        {
            subscribers[eventType] = new List<Delegate>();
        }

        subscribers[eventType].Add(action);
    }

    public static void UnSubscribe<TEventData>(Action<TEventData> action)
    {
        Type eventType = typeof(TEventData);

        if (subscribers.ContainsKey(eventType))
        {
            subscribers[eventType].Remove(action);
        }
    }

    public static void Publish<TEventData>(TEventData eventData)
    {
        Type eventType = typeof(TEventData);

        if (subscribers.ContainsKey(eventType))
        {
            var subscriberList = subscribers[eventType];
            var copiedList = new List<Delegate>(subscriberList);

            foreach (var wrapper in copiedList)
            {
                if (wrapper is Action<TEventData> action)
                {
                    action.Invoke(eventData);
                }
            } 
        }
    }
}
