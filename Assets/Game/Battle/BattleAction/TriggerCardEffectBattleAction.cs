public class TriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private TargetBattleEntity targetEntity;

    public TriggerCardEffectBattleAction(Card card, TargetBattleEntity targetEntity)
    {
        this.card = card;
        this.targetEntity = targetEntity;
    }

    public Card Card { get => card; }
    public TargetBattleEntity TargetEntity { get => targetEntity; }

    public void Execute(BattleContext context)
    {
        context.ActionScheduler.Enqueue(new UseCardEffectBattleAction(card, targetEntity));
    }
}