#nullable enable

using System;
using System.Collections.Generic;

public interface IHandDeckContext : IReadOnlyBattleDeck{
    public Card? GetRandomCard(Random random, ICardBehaviourOwner? ignoringCardBehaviourOwner = null);
    public List<Card> GetCardsByCondition(CardRarity rarity, CardAttribute attribute, CardType type);
}