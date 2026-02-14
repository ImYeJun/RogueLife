using System.Collections.Generic;

public interface IFieldCardDatabase
{
    public Card GetRandomCard(System.Random random, CardRarity rarity, CardType type, CardAttribute attribute);
    public Card Materialize(CardData data);
    public List<Card> GetEnemyResolveReward(System.Random random, CardEnemyResolveReward data);
}