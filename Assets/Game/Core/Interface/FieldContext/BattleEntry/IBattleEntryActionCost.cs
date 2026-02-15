public interface IBattleEntryActionCost
{
    public int MaxActionCost { get; }
    public void OnBattleEnd(BattleResult result);
}