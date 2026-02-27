public class IncreasePhaseCountBattleAction : IBattleAction
{
    private int amount;

    public IncreasePhaseCountBattleAction(int amount)
    {
        this.amount = amount;
    }

    public int Amount { get => amount; }

    public void Execute(BattleContext context)
    {
        context.Phase.Increase(amount);
    }
}