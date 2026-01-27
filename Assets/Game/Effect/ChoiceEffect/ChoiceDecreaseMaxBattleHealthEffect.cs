public class ChoiceDecreaseMaxBattleHealthEffect : IChoiceEffect
{
    private int amount;

    public ChoiceDecreaseMaxBattleHealthEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.DecreaseMaxBattleHealth(amount);
    }
}