#if UNITY_EDITOR
using View.BattleView;
public partial class BattleSystem
{
    public void TestHurtEnemy(BattleEnemyView view, int amount)
    {
        enemySystem.TestHurtEnemy(view, amount);
    }

    public void TestHealEnemy(BattleEnemyView view, int amount)
    {
        enemySystem.TestHealEnemy(view, amount);
    }
}
#endif