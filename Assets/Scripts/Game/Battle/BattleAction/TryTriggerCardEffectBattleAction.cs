public class TryTriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardTarget cardTarget;
    private int executeTimes;
    private bool isReflection;

    public TryTriggerCardEffectBattleAction(Card card, CardTarget cardTarget, int executeTimes = 1, bool isReflection = false)
    {
        this.card = card;
        this.cardTarget = cardTarget;
        this.executeTimes = executeTimes;
        this.isReflection = isReflection;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => cardTarget; }

    public void Execute(BattleContext context)
    {
        if (card.IsAbleToUse(context, cardTarget))
        {
            context.ActionScheduler.Enqueue(new TriggerCardEffectBattleAction(card, cardTarget, executeTimes, isReflection));
        }
    }
}