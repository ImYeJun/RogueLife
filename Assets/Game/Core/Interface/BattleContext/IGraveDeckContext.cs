using System;
using System.Collections.Generic;

public interface IGraveDeckContext {
    public bool HasCard(Card card);
    public List<Card> GetCards();
    public Card GetRandomCard(Random random);
    public Card GetRandomCard(Random random, CardRarity rarity, CardAttribute attribite, CardType type);
    public List<Card> GetCardsByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
    public int GetCardsCountByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
}