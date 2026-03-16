#nullable enable

using System;

public interface IBattleDeckSystemContext {
    public void MoveCard(Card card, BattleDeckType destination);
    public Card? RequestDrawingCard(Random random, CardRarity rarity, CardAttribute attribute, CardType type);
    public void ReviveGraveCards(bool insertFront = false);
    public void RequestUseCard(Card card, bool isFreeUse);
    public void RequestTriggerCard(Card card, bool isReflection);
    public void AddActiveTriggerCard(Card card);
    public void RemoveActiveTriggerCard(Card card);
}