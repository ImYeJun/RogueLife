using System;

public partial class GameRun
{
#if UNITY_EDITOR
    public void TestAddBelongings(BelongingsEntity entity)
    {
        var newBelongings = belongingsDatabase.Materialize(entity);
        player.BelongingsBag.TryObtainBelongings(newBelongings);
    }

    public void TestAddCard(CardEntity entity)
    {
        var newCard = cardDatabase.Materialize(entity);
        player.Deck.TryObtainCard(newCard);
    }
#endif
}