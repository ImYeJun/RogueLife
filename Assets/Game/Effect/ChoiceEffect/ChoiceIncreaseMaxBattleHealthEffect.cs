public class ChoiceIncreaseMaxBattleHealthEffect : IChoiceEffect
{
    private int amount;

    public ChoiceIncreaseMaxBattleHealthEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.IncreaseMaxBattleHealth(amount);
    }
}