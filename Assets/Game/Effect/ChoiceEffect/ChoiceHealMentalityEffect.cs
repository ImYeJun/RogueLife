public class ChoiceHealMentalityEffect : IChoiceEffect
{
    private int amount;

    public ChoiceHealMentalityEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.HealMentality(amount);
    }
}