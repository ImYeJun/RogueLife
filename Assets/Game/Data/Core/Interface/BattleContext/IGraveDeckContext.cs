using System;
using System.Collections.Generic;

public interface IGraveDeckContext {
    public bool HasCard(Card card);
    public List<Card> GetCards();
    public Card GetRandomCard(Random random);
    public Card GetRandomCard(Random random, CardAttribute attribite, CardType type);
    public List<Card> GetCardsByCondition(CardAttribute attribute, CardType type);
    public int GetCardsCountByCondition(CardAttribute attribute, CardType type);
}