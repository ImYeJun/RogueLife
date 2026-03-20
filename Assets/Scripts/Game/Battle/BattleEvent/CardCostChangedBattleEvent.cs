public class CardCostChangedBattleEvent : BattleEvent
{
    private Card card;
    private int currentCost;

    public CardCostChangedBattleEvent(Card card, int currentCost)
    {
        this.card = card;
        this.currentCost = currentCost;
    }

    public Card Card { get => card; }
    public int CurrentCost { get => currentCost; }
}