public class TryTriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private TargetBattleEntity targetEntity;

    public TryTriggerCardEffectBattleAction(Card card, TargetBattleEntity targetEntity)
    {
        this.card = card;
        this.targetEntity = targetEntity;
    }

    public Card Card { get => card; }
    public TargetBattleEntity TargetEntity { get => targetEntity; }

    public void Execute(BattleContext context)
    {
        if (card.IsAbleToUse(context))
        {
            context.ActionScheduler.Enqueue(new TriggerCardEffectBattleAction(card, targetEntity));
        }
    }
}