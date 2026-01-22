using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static Dictionary<Type, List<Delegate>> subscribers;

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
            List<Delegate> copiedList = subscribers[eventType];

            foreach (Action<TEventData> action in copiedList)
            {
                action.Invoke(eventData);
            } 
        }
    }
}
