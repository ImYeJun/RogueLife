public class MoveCardToDeckBattleAction : IBattleAction
{
    private Card card;
    private BattleDeckType destination;

    public MoveCardToDeckBattleAction(Card card, BattleDeckType destination)
    {
        this.card = card;
        this.destination = destination;
    }

    public Card Card { get => card; }
    public BattleDeckType Destination { get => destination; }

    public void Execute(BattleContext context)
    {
        context.DeckSystem.MoveCard(card, destination);
    }
}