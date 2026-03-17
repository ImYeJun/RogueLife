public interface IBattleViewCommander : IViewCommander
{
    public bool IsAbleToUseCard(Card card, CardTarget cardTarget);
    public void StartBattle();
    public void EndPlayerTurn();
    public void UseCard(Card card, CardTarget cardTarget, bool isFreeUse);
    public void TriggerCard(Card card, CardTarget cardTarget, bool isReflection);
    public void CancelActivation(Card card, bool isTriggering);
    public BattleStatusEffectData GetStatusEffectData(string id);
}