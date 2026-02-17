public interface IBattleStatusEffectState
{
    public int StackCount { get; }
    public int RemainTurn { get; }
    public bool IsDurationEternal { get; }
    public void RequestExpired();
}