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
        for (int i = 0; i < executeTimes; i++)
        {
            card.Use(context, caster, target);
            context.BattleDeckHistory.RecordUseCard(card, card.IsReflectionApplied);
        }

        var destination = card.IsReflectionApplied ? BattleDeckType.DRAW : BattleDeckType.GRAVE;

        if (card.IsReflectionApplied) { card.UnapplyReflection(); }
        
        context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(card, destination));
    }
}