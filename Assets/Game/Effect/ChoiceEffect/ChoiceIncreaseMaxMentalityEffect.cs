public class ChoiceIncreaseMaxMentalityEffect : IChoiceEffect
{
    private int amount;

    public ChoiceIncreaseMaxMentalityEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.IncreaseMaxMentality(amount);
    }
}