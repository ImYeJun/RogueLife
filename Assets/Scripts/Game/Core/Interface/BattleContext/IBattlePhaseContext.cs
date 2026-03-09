public interface IBattlePhaseContext : IReadOnlyBattlePhase{
    public bool IsAllPhasedEnd { get; }
    public void Increase(int amount);
    public void Decrease(int amount);
}