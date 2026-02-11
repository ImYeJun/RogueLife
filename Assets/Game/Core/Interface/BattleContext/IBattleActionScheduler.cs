public interface IBattleActionScheduler {
    public void Enqueue(IBattleAction action);
    public void PushActionScope(BattleActionScope scope);
}