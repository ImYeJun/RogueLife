public interface IBattleActionCost {
    public int RemainCost { get; }
    public bool HasEnough(int amount);
    public void Consume(int amount);
    public void Restore(int amount);
    public void Fullfill();
}