#nullable enable

using System.Collections.Generic;

public interface IBattleCardDatabase
{
    public Card? GetRandomCard(System.Random random, CardRarity rarity, CardType type, CardAttribute attribute, List<CardData>? ignoringCardData = null);
    public Card? GetRandomCard(System.Random random, CardRarity minRarity, CardRarity maxRarity, CardType type, CardAttribute attribute, List<CardData>? ignoringCardData = null);
}