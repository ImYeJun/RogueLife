using Battle.Cards.Casters;

public class TriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardTarget targetEntity;
    private int executeTimes;
    private bool isReflection;

    public TriggerCardEffectBattleAction(Card card, CardTarget targetEntity, int executeTimes, bool isReflection = false)
    {
        this.card = card;
        this.targetEntity = targetEntity;
        this.executeTimes = executeTimes;
        this.isReflection = isReflection;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => targetEntity; }
    public int ExecuteTimes { get => executeTimes; }

    public void Execute(BattleContext context)
    {
        var caster = new NoneEntityCaster();

        context.DeckSystem.AddActiveTriggerCard(card);

        if (executeTimes > 0)
        {
            context.EventBus.Publish(new CardEffectExecutedBattleEvent(card, caster, targetEntity));
            card.Trigger(context, caster, targetEntity, isReflection);
            context.BattleDeckHistory.RecordExecuteCardEffect(card, isReflection);
        }

        if (executeTimes > 1)
        {
            context.ActionScheduler.Enqueue(new TriggerCardEffectBattleAction(card, targetEntity, executeTimes - 1, isReflection));
        }
        else
        {
            context.ActionScheduler.Enqueue(new ResolveCardTriggerBattleAction(card));
            context.ActionScheduler.Enqueue(new NotifyCardExecutionCompletedBattleAction(card));
        }
    }
}