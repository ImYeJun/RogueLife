using System;

public class DrawDeck : IDrawDeckContext
{
    public bool HasCard(Card card)
    {
        throw new NotImplementedException();
    }

    public Card RequestDrawCard(CardAttribute attribute, CardType type)
    {
        throw new NotImplementedException();
    }
}