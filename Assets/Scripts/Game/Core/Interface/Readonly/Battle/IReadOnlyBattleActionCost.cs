public interface IReadOnlyBattleActionCost {
    public bool HasEnough(int amount);
    public int RemainCost { get; }
    public int MaxActionCost { get; }
}