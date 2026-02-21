using System.Collections.Generic;

public interface IFieldCardDatabase : IBattleCardDatabase
{
    public Card Materialize(CardData data);
    public List<Card> GetEnemyResolveReward(System.Random random, CardEnemyResolveReward data);
}