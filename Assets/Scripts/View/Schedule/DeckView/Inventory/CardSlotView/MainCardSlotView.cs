namespace View.ScheduleView.Deck
{
    public class MainCardSlotView : CardSlotView
    {
        protected override void OnFocusedClicked()
        {
            commander.MoveCard(CurrentCard, DeckType.MAIN_DECK, DeckType.SIDE_DECK);
        }
    }
}