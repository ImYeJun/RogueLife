#if UNITY_EDITOR
using System.Linq;
using View.BattleView;
using Battle.HurtSources; 

public partial class BattleEnemySystem
{
    public void TestHurtEnemy(BattleEnemyView view, int amount)
    {
        if (view == null)
        {
            UnityEngine.Debug.LogWarning("[BattleEnemySystem/TestHurtEntity] The provided BattleEnemyView is null.");
            return;
        }

        BattleEnemy targetEnemy = currentEnemies.Values
            .SelectMany(list => list)
            .FirstOrDefault(e => e == view.Enemy);

        if (targetEnemy != null)
        {
            targetEnemy.ReceiveDamage(amount, new NoneEntitySource());
        }
        else
        {
            UnityEngine.Debug.LogWarning("[BattleEnemySystem/TestHurtEntity] Target enemy not found in current enemies.");
        }
    }

    public void TestHealEnemy(BattleEnemyView view, int amount)
    {
        if (view == null)
        {
            UnityEngine.Debug.LogWarning("[BattleEnemySystem/TestHealEntity] The provided BattleEnemyView is null.");
            return;
        }

        BattleEnemy targetEnemy = currentEnemies.Values
            .SelectMany(list => list)
            .FirstOrDefault(e => e == view.Enemy);

        if (targetEnemy != null)
        {
            targetEnemy.Heal(amount);
        }
        else
        {
            UnityEngine.Debug.LogWarning("[BattleEnemySystem/TestHealEntity] Target enemy not found in current enemies.");
        }
    }
}
#endif