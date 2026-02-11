using System;
using System.Collections.Generic;

public class GraveDeck : IGraveDeckContext
{
    public bool HasCard(Card card)
    {
        throw new NotImplementedException();
    }

    public HashSet<Card> GetCards()
    {
        throw new NotImplementedException();
    }

    public Card GetRandomCard()
    {
        throw new NotImplementedException();
    }

    public HashSet<Card> GetCardsByCondition(CardAttribute attribute, CardType type)
    {
        throw new NotImplementedException();
    }

    public int GetCardsCountByCondition(CardAttribute attribute, CardType type)
    {
        throw new NotImplementedException();
    }
}