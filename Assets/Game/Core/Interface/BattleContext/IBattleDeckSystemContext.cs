using System;

public interface IBattleDeckSystemContext {
    public void MoveCard(Card card, BattleDeckType destination);
    public Card RequestDrawingCard(Random random, CardRarity rarity, CardAttribute attribute, CardType type);
    public void ReviveGraveCards(bool insertFront = false);
}