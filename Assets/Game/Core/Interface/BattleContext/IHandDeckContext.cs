using System.Collections.Generic;

public interface IHandDeckContext {
    public bool HasCard(Card card);
    public int GetCardsCountByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
    public List<Card> GetCardsByCondition(CardRarity rarity, CardAttribute attribute, CardType type);
    public List<Card> GetCards();
}