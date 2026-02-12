public class UseCardBattleAction : IBattleAction
{
    private Card card;
    private CardTarget cardTarget;

    public UseCardBattleAction(Card card, CardTarget cardTarget)
    {
        this.card = card;
        this.cardTarget = cardTarget;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => cardTarget; }

    public void Execute(BattleContext context)
    {
        var cardEffectAction = new UseCardEffectBattleAction(card, cardTarget);
        context.ActionScheduler.Enqueue(new BattleEntityAction(context.PlayerContainer.Player, cardEffectAction));

        //* It says this code is limited in scalability. If new features are needed, this code may be refactored.
        if (card.IsReflectionApplied)
        {
            context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(card, BattleDeckType.DRAW));
        }
        else
        {
            context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(card, BattleDeckType.GRAVE));
        }
    }
}