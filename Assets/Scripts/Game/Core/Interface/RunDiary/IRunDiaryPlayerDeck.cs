using System.Collections.Generic;

public interface IRunDiaryPlayerDeck
{
    public Dictionary<CardData, List<Card>> GetClonedMainDeck(bool isBattle = false);
    public Dictionary<CardData, List<Card>> GetClonedSideDeck();
}