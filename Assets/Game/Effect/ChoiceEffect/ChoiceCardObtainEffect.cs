public class ChoiceCardObtainEffect : IChoiceEffect
{
    private Card obtainingCard;

    public ChoiceCardObtainEffect(Card obtainingCard)
    {
        this.obtainingCard = obtainingCard;
    }

    public void Execute(ChoiceContext context)
    {
        context.Deck.TryObtainCard(obtainingCard);
    }
}