using Battle.Cards.Casters;

public class UseCardBattleAction : IBattleAction
{
    private Card card;
    private CardTarget target;

    public UseCardBattleAction(Card card, CardTarget target)
    {
        this.card = card;
        this.target = target;
    }

    public Card Card { get => card; }
    public CardTarget Target { get => target; }

    public void Execute(BattleContext context)
    {
        var caster = new EntityCardCaster(context.PlayerContainer.Player);
        var cardEffectAction = new UseCardEffectBattleAction(card, caster, target);
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