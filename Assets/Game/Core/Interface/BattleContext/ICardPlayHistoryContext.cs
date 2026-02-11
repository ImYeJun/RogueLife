using System.Collections.Generic;

public interface ICardPlayHistoryContext {
    public bool HasPlayedConditionedCard(CardAttribute attribite, CardType type, BattleScope scope);
    public bool HasPlayedCard(BattleScope scope);
    public int GetPlayedCardCount(BattleScope scope);
    public HashSet<Card> GetRecentlyGravedCard(int amount);
    public Card GetRecentlyPlayedCard();
}