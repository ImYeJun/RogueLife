public interface IScheduleViewCommander : IViewCommander
{
    public void BroadcastCurrentState();
    public void EnterStartNodeIfNeeded();
    public void MoveBelonings(Belongings belongings, BelongingsBagType mAIN_BELONGINGS_BAG, BelongingsBagType sIDE_BELONGINGS_BAG);
    public void MoveCard(Card card, DeckType from, DeckType to);
}