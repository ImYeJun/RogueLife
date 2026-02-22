#nullable enable

using System.Collections.Generic;

public interface IBattleDeckHistoryContext {
    public void RecordUseCard(Card card, bool isReflection = false);
    public CardPlayHistory? GetRecentlyPlayedHistory(ICardBehaviourOwner? ignoringCardBehaviourOwner = null);
    public List<CardPlayHistory>? GetRecentPhasePlayedHistory(ICardBehaviourOwner? ignoringCardBehaviourOwner = null);
    public bool HasPlayedCard(CardRarity rarity, CardAttribute attribite, CardType type, BattleScope scope);
    public bool HasPlayedCard(BattleScope scope);
    public int GetPlayedCardCount(BattleScope scope);
    public List<Card> GetRecentlyGravedCard(int amount);
}