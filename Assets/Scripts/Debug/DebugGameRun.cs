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

    public void TestHurtPlayer(int testHurtDamage, bool isOverflowable)
    {
        player.Health.HurtBattleHealth(testHurtDamage, isOverflowable);
    }

    public void TestHealMentality(int testHealAmount, bool isOverflowable)
    {
        player.Health.HealMentality(testHealAmount, isOverflowable);
    }

    public void TestHealBattleHealth(int testHealAmount)
    {
        player.Health.HealBattleHealth(testHealAmount);
    }
#endif
}