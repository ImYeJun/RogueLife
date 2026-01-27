public class ChoiceHealBattleHealthEffect : IChoiceEffect
{
    private int amount;

    public ChoiceHealBattleHealthEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.HealBattleHealth(amount);
    }
}