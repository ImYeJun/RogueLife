using System.Collections.Generic;

public interface IBattleDeckHistoryContext {
    public void RecordUseCard(Card card);
    public Card GetRecentlyPlayedCard();
    public bool HasPlayedCard(CardRarity rarity, CardAttribute attribite, CardType type, BattleScope scope);
    public bool HasPlayedCard(BattleScope scope);
    public int GetPlayedCardCount(BattleScope scope);
    public List<Card> GetRecentlyGravedCard(int amount);
}