public interface IDrawDeckContext {
    public bool HasCard(Card card);
    public int GetCardsCountByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
}