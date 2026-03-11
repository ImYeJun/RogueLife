public interface IBattlePhaseContext : IReadOnlyBattlePhase{
    public bool IsAllTurnEnd { get; }
    public void Increase(int amount);
    public void Decrease(int amount);
}