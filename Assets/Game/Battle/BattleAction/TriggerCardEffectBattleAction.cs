using Battle.Cards.Casters;

public class TriggerCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardTarget targetEntity;
    private int executeTimes;

    public TriggerCardEffectBattleAction(Card card, CardTarget targetEntity, int executeTimes = 1)
    {
        this.card = card;
        this.targetEntity = targetEntity;
        this.executeTimes = executeTimes;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => targetEntity; }
    public int ExecuteTimes { get => executeTimes; set => executeTimes = value; }

    public void Execute(BattleContext context)
    {
        context.ActionScheduler.Enqueue(new UseCardEffectBattleAction(card, new NoneEntityCaster(), targetEntity, executeTimes));
    }
}