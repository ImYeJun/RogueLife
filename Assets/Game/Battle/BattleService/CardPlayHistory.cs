using System;
using System.Collections.Generic;

public class CardPlayHistory : ICardPlayHistoryContext
{
    public bool HasPlayedConditionedCard(CardAttribute attribute, CardType type, BattleScope scope)
    {
        throw new NotImplementedException();
    }
    
    public bool HasPlayedCard(BattleScope scope)
    {
        throw new NotImplementedException();
    }

    public int GetPlayedCardCount(BattleScope scope)
    {
        throw new NotImplementedException();
    }

    public HashSet<Card> GetRecentlyGravedCard(int amount)
    {
        throw new NotImplementedException();
    }

    public Card GetRecentlyPlayedCard()
    {
        throw new NotImplementedException();
    }
}