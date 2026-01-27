public class ChoiceReceiveDamageEffect : IChoiceEffect
{
    private int amount;

    public ChoiceReceiveDamageEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Health.ReceiveDamage(amount);
    }
}