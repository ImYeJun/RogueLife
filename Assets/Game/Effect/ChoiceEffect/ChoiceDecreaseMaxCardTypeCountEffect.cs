public class ChoiceDecreaseMaxCardTypeCountEffect : IChoiceEffect
{
    private int amount;

    public ChoiceDecreaseMaxCardTypeCountEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Deck.DecreaseMaxCardTypeCount(amount);
    }
}