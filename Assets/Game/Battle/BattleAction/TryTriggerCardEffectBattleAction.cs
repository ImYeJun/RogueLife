public class TryTriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardTarget cardTarget;

    public TryTriggerCardEffectBattleAction(Card card, CardTarget cardTarget)
    {
        this.card = card;
        this.cardTarget = cardTarget;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => cardTarget; }

    public void Execute(BattleContext context)
    {
        if (card.IsAbleToUse(context, cardTarget))
        {
            context.ActionScheduler.Enqueue(new TriggerCardEffectBattleAction(card, cardTarget));
        }
    }
}