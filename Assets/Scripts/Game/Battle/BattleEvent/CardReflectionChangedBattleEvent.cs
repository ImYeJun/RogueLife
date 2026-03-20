public class CardReflectionChangedBattleEvent : BattleEvent
{
    private Card card;
    private bool isReflection;

    public CardReflectionChangedBattleEvent(Card card, bool isReflection)
    {
        this.card = card;
        this.isReflection = isReflection;
    }

    public Card Card { get => card; set => card = value; }
    public bool IsReflection { get => isReflection; set => isReflection = value; }
}