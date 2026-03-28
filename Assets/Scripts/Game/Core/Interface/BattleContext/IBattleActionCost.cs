public interface IBattleActionCost : IReadOnlyBattleActionCost {
    public void Consume(int amount);
    public void Restore(int amount);
    public void Fullfill();
    public void AddModifier(BattleMaxActionCostModifier modifier);
}