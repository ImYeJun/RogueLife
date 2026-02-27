public class ConsumeActionCostBattleAction : IBattleAction
{
    private int amount;

    public ConsumeActionCostBattleAction(int amount)
    {
        this.amount = amount;
    }

    public int Amount { get => amount; }

    public void Execute(BattleContext context)
    {
        context.ActionCost.Consume(amount);
    }
}