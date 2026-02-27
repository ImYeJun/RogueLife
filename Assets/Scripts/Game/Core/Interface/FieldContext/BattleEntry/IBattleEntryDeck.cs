using System.Collections.Generic;

public interface IBattleEntryDeck
{
    public Dictionary<CardData, List<Card>> GetClonedMainDeck(bool isForBattleStart = false);
}