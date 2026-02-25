public interface IBattleEntryActionCost
{
    public int CurrentMaxActionCost { get; }
    public void OnBattleEnd();
}