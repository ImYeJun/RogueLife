using Battle.Cards.Casters;

public class UseCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardCaster caster;
    private CardTarget target;
    private int executeTimes;

    public UseCardEffectBattleAction(Card card, CardCaster caster, CardTarget target, int executeTimes)
    {
        this.card = card;
        this.caster = caster;
        this.target = target;
        this.executeTimes = executeTimes;
    }

    public Card Card { get => card; }
    public CardCaster Caster { get => caster; }

    public void Execute(BattleContext context)
    {
        if (executeTimes > 0)
        {
            context.EventBus.Publish(new CardEffectExecutedBattleEvent(card, caster, target));
            card.Use(context, caster, target);
            context.BattleDeckHistory.RecordExecuteCardEffect(card, card.IsReflectionApplied);
        }

        if (executeTimes > 1)
        {
            context.ActionScheduler.Enqueue(new UseCardEffectBattleAction(card, caster, target, executeTimes - 1));
        }
        else
        {
            var destination = card.IsReflectionApplied ? BattleDeckType.DRAW : BattleDeckType.GRAVE;
            context.BattleDeckHistory.RecordUseCard(card, card.IsReflectionApplied);

            context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(card, destination));
            if (card.IsReflectionApplied) { context.ActionScheduler.Enqueue(new UnapplyReflectEffectOnCardBattleAction(card)); }
            context.ActionScheduler.Enqueue(new NotifyCardExecutionCompletedBattleAction(card));
        }
    }
}