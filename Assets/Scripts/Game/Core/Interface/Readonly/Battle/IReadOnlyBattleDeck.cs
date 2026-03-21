using System.Collections.Generic;

public interface IReadOnlyBattleDeck {
    public List<Card> GetCards();
    public int Count { get; }
    public bool HasCard(Card card);
    public int GetCardsCountByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
}