using System.Collections.Generic;

public class BattleEventBus : IBattleEventBus
{
    private HashSet<IBattleEventObserver> observers = new HashSet<IBattleEventObserver>();
    
    public void Publish(BattleEvent battleEvent)
    {
        foreach (var observer in observers)
        {
            observer.OnBattleEvent(battleEvent);
        }
    }

    public void Subscribe(IBattleEventObserver observer)
    {
        if (!observers.Add(observer))
        {
            UnityEngine.Debug.LogWarning("The given observer is already subscribing Battle Event bus.");
        }
    }

    public void Unsubscribe(IBattleEventObserver observer)
    {
        if (!observers.Remove(observer))
        {
            UnityEngine.Debug.LogError("The given observer is not subscribing Battle Event bus.");
        }
    }
}