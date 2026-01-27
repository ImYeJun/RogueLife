public class ChoiceCardRemoveEffect : IChoiceEffect
{
    private Card removingCard;

    public ChoiceCardRemoveEffect(Card removingCard)
    {
        this.removingCard = removingCard;
    }

    public void Execute(ChoiceContext context)
    {
        context.Deck.TryRemoveCard(removingCard);
    }
}