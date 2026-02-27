using Battle.Cards.Casters;

public class UseCardBattleAction : IBattleAction
{
    private Card card;
    private CardTarget target;
    private int executeTimes;

    public UseCardBattleAction(Card card, CardTarget target, int executeTimes = 1)
    {
        this.card = card;
        this.target = target;
        this.executeTimes = executeTimes;
    }

    public Card Card { get => card; }
    public CardTarget Target { get => target; }
    public int ExecuteTimes { get => executeTimes; set => executeTimes = value; }

    public void Execute(BattleContext context)
    {
        var caster = new EntityCardCaster(context.PlayerContainer.Player);
        var cardEffectAction = new UseCardEffectBattleAction(card, caster, target, executeTimes);
        context.ActionScheduler.Enqueue(new BattleEntityAction(context.PlayerContainer.Player, cardEffectAction));
    }
}