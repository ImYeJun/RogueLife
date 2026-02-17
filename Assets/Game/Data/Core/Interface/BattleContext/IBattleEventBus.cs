public interface IBattleEventBus {
    public void Publish(BattleEvent battleEventevent);
    public void Subscribe(IBattleEventObserver observer);
    public void Unsubscribe(IBattleEventObserver observer);
}