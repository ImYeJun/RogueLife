
public interface IScheduleViewCommander : IViewCommander
{
    public void BroadcastCurrentState();
    public void ResumeSchedule();
    public void MoveBelonings(Belongings belongings, BelongingsBagType mainBag, BelongingsBagType sideBag);
    public void MoveCard(Card card, DeckType from, DeckType to);
    public void SettleNextNode(Node nextNode);
    public void SettleTransactionChoice(TransactionChoiceOrder order);
    public void SettleIncidentChoice(DeterminedIncidentChoice choice);
    public BattleStatusEffectData GetStatusEffectData(string id);
}