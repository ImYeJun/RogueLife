#nullable enable

using System;
using System.Collections.Generic;

public interface IGraveDeckContext : IReadOnlyBattleDeck{
    public Card? GetRandomCard(Random random, ICardBehaviourOwner? ignoringCardBehaviourOwner = null);
    public Card? GetRandomCard(Random random, CardRarity rarity, CardAttribute attribite, CardType type, ICardBehaviourOwner? ignoringCardBehaviourOwner = null);
    public List<Card> GetCardsByCondition(CardRarity rarity, CardAttribute attribite, CardType type);
}