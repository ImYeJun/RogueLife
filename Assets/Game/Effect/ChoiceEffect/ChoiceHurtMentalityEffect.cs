public class ChoiceHurtMentalityEffect : IChoiceEffect
{
    private int amount;

    public ChoiceHurtMentalityEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.HurtMentality(amount);
    }
}