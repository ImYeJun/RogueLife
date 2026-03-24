using View.Global;

namespace View.ScheduleView.Deck
{
    public class SideCardSlotView : CardSlotView
    {
        protected override void OnFocusedClicked()
        {
            commander.MoveCard(CurrentCard, DeckType.SIDE_DECK, DeckType.MAIN_DECK);
        }
    }
}