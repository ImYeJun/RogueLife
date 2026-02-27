#nullable enable

using System;
using System.Collections.Generic;

public interface IHandDeckContext {
    public int Count { get; }
    public bool HasCard(Card card);
    public int GetCardsCountByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
    public Card? GetRandomCard(Random random, ICardBehaviourOwner? ignoringCardBehaviourOwner = null);
    public List<Card> GetCardsByCondition(CardRarity rarity, CardAttribute attribute, CardType type);
    public List<Card> GetCards();
}