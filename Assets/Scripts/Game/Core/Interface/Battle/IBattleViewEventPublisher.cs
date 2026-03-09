using ViewEvent.BattleView;

public interface IBattleViewEventPublisher {
    public void Publish<T>(T payload) where T : IBattleViewEvent;
    public int GetNextSequenceId();
}