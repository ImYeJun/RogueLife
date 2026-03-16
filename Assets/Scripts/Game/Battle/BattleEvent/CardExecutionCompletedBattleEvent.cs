public class CardExecutionCompletedBattleEvent : BattleEvent
{
    public Card Card { get; private set; }

    public CardExecutionCompletedBattleEvent(Card card) { Card = card; }
}