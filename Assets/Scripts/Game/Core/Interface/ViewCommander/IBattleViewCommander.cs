public interface IBattleViewCommander : IViewCommander
{
    public bool IsAbleToUseCard(Card card, CardTarget cardTarget);
    public void StartBattle();
    public void UseCard(Card card, CardTarget cardTarget);
}