using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Scriptable Objects/EnemyData/Boss")]
public class BossEnemyData : EnemyData {
    private void Reset() {
        tier = EnemyTier.BOSS;
        lossMentalityOnUnresolved = Constant.BOSS_ENEMY_MENTALITY_PENALTY_AMOUNT;
        reward = new CardEnemyResolveReward(CardRarity.RARE, CardRarity.LEGENDARY, 2);
    }
}