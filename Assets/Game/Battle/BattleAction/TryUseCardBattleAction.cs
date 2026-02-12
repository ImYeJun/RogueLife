public class TryUseCardBattleAction : IBattleAction
{
    private int cost;
    private Card card;
    private TargetBattleEntity targetEntity;

    public TryUseCardBattleAction(int cost, Card card, TargetBattleEntity targetEntity)
    {
        this.cost = cost;
        this.card = card;
        this.targetEntity = targetEntity;
    }

    public int Cost { get => cost; }
    public Card Card { get => card; }
    public TargetBattleEntity TargetEntity { get => targetEntity; }

    public void Execute(BattleContext context)
    {
        if (!card.IsAbleToUse(context)) { return; }
        if (!context.ActionCost.HasEnough(cost)) { return; }

        //* If race condition problem happens, refactor this code to work synchronously.
        context.ActionScheduler.Enqueue(new ConsumeActionCostBattleAction(cost));
        context.ActionScheduler.Enqueue(new UseCardBattleAction(card, targetEntity));
    }
}