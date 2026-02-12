using System;

public interface IBattleDeckSystemContext {
    public void MoveCard(Card card, BattleDeckType destination);
    public Card RequestDrawingCard(Random random, CardAttribute attribute, CardType type);
}