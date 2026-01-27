public class ChoiceDecreaseMaxMentalityEffect : IChoiceEffect
{
    private int amount;

    public ChoiceDecreaseMaxMentalityEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.DecreaseMaxMentality(amount);
    }
}