public class ChoiceIncreaseMaxCardTypeCountEffect : IChoiceEffect
{
    private int amount;
    
    public ChoiceIncreaseMaxCardTypeCountEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.Deck.IncreaseMaxCardTypeCount(amount);
    }
}