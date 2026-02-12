public class UseCardBattleAction : IBattleAction
{
    private Card card;
    private TargetBattleEntity targetEntity;

    public UseCardBattleAction(Card card, TargetBattleEntity targetEntity)
    {
        this.card = card;
        this.targetEntity = targetEntity;
    }

    public Card Card { get => card; }

    public void Execute(BattleContext context)
    {
        var cardEffectAction = new UseCardEffectBattleAction(card, targetEntity);
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