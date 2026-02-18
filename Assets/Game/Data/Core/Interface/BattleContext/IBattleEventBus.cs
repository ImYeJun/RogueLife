using System;

public interface IBattleEventBus {
    public void Publish<T>(T battleEvent) where T : BattleEvent;
    public void Subscribe<T>(Action<T> observer, BattleEventObserverStage stage = BattleEventObserverStage.MIDDLE) where T : BattleEvent;
    public void Unsubscribe<T>(Action<T> observer) where T : BattleEvent;
}