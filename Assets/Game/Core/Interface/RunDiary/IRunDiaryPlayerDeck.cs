using System.Collections.Generic;

public interface IRunDiaryPlayerDeck
{
    public Dictionary<CardData, List<Card>> GetClonedMainDeck();
    public Dictionary<CardData, List<Card>> GetClonedSideDeck();
}