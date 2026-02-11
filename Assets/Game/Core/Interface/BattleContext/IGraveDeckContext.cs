using System.Collections.Generic;

public interface IGraveDeckContext {
    public bool HasCard(Card card);
    public HashSet<Card> GetCards();
    public Card GetRandomCard();
    public HashSet<Card> GetCardsByCondition(CardAttribute attribute, CardType type);
    public int GetCardsCountByCondition(CardAttribute attribute, CardType type);
}