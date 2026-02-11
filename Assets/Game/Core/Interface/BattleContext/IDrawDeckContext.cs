public interface IDrawDeckContext {
    public bool HasCard(Card card);
    public Card RequestDrawCard(CardAttribute attribite, CardType type);
}