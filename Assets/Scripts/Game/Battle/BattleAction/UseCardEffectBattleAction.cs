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
        var destination = card.IsReflectionApplied ? BattleDeckType.DRAW : BattleDeckType.GRAVE;
        if (card.IsReflectionApplied) { card.UnapplyReflection(); }

        for (int i = 0; i < executeTimes; i++)
        {
            context.EventBus.Publish(new CardEffectExecutedBattleEvent(card, caster, target));
            card.Use(context, caster, target);
            context.BattleDeckHistory.RecordExecuteCardEffect(card, card.IsReflectionApplied);
        }
        context.BattleDeckHistory.RecordUseCard(card, card.IsReflectionApplied);

        context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(card, destination));
        context.ActionScheduler.Enqueue(new NotifyCardExecutionCompletedBattleAction(card));
    }
}