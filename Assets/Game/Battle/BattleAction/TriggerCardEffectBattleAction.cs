public class TriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardTarget targetEntity;

    public TriggerCardEffectBattleAction(Card card, CardTarget targetEntity)
    {
        this.card = card;
        this.targetEntity = targetEntity;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => targetEntity; }

    public void Execute(BattleContext context)
    {
        context.ActionScheduler.Enqueue(new UseCardEffectBattleAction(card, targetEntity));
    }
}