public class DecreasePhaseCountBattleAction : IBattleAction
{
    private int amount;

    public DecreasePhaseCountBattleAction(int amount)
    {
        this.amount = amount;
    }

    public int Amount { get => amount; }

    public void Execute(BattleContext context)
    {
        context.Phase.Decrease(amount);
    }
}