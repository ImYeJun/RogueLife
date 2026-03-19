using System;
using View.BattleView;

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

    public void TestRemoveCard(CardEntity entity)
    {
        player.Deck.TryRemoveCardByData(entity.Data, 1);
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

    public void TestHurtEnemy(BattleEnemyView view, int amount)
    {
        battleSystem?.TestHurtEnemy(view, amount);
    }

    public void TestHealEnemy(BattleEnemyView view, int amount)
    {
        battleSystem?.TestHealEnemy(view, amount);
    }
#endif
}