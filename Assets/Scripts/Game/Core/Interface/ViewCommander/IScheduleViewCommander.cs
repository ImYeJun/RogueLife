public interface IScheduleViewCommander : IViewCommander
{
    public void BroadcastCurrentState();
    public void EnterStartNodeIfNeeded();
    public void MoveCard(Card card, DeckType from, DeckType to);
}