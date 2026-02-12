public class RestoreActionCostBattleAction : IBattleAction
{
    private int amount;

    public RestoreActionCostBattleAction(int amount)
    {
        this.amount = amount;
    }
    public int Amount { get => amount; }

    public void Execute(BattleContext context)
    {
        context.ActionCost.Restore(amount);
    }
}