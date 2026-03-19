using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleEventBus : IBattleEventBus
{
    private struct ObserverWrapper
    {
        public Delegate action;
        public BattleEventObserverStage stage;
    }
    
    private Dictionary<Type, List<ObserverWrapper>> observers = new Dictionary<Type, List<ObserverWrapper>>();

    public void Publish<T>(T battleEvent) where T : BattleEvent
    {
        var eventType = typeof(T);
        
        if (!observers.TryGetValue(eventType, out var wrapperList) || wrapperList.Count == 0)
        {
            return;
        }

        var snapshot = wrapperList.ToList();
        for (int i = snapshot.Count - 1; i >= 0; i--)
        {
            var materializedAction = (Action<T>)snapshot[i].action;
            materializedAction.Invoke(battleEvent);
        }
    }

    public void Subscribe<T>(Action<T> observer, BattleEventObserverStage stage = BattleEventObserverStage.MIDDLE) where T : BattleEvent
    {
        var eventType = typeof(T);
        if (!observers.ContainsKey(eventType)) 
        { 
            observers[eventType] = new List<ObserverWrapper>(); 
        }

        var wrapperList = observers[eventType];
        if (wrapperList.Any(wrapper => wrapper.action == (Delegate)observer))
        {
            Debug.LogWarning("[BattleEventBus] The given observer is already subscribing.");
            return;
        }

        wrapperList.Add(new ObserverWrapper { action = observer, stage = stage });
        wrapperList.Sort((a, b) => b.stage.CompareTo(a.stage));
    }

    public void Unsubscribe<T>(Action<T> observer) where T : BattleEvent
    {
        var eventType = typeof(T);
        if (!observers.TryGetValue(eventType, out var wrapperList) || wrapperList.Count == 0)
        {
            Debug.LogWarning("[BattleEventBus] The given observer is not subscribing.");
            return;
        }

        int removedCount = wrapperList.RemoveAll(wrapper => wrapper.action == (Delegate)observer);
        if (removedCount == 0)
        {
            Debug.LogWarning("[BattleEventBus] The given observer is not subscribing.");
        }
    }
}