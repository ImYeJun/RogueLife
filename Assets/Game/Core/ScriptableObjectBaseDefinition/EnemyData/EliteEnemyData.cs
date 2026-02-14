using UnityEngine;

[CreateAssetMenu(fileName = "EliteEnemyData", menuName = "Scriptable Objects/EnemyData/Elite")]
public class EliteEnemyData : EnemyData {
    private void Reset() {
        tier = EnemyTier.ELITE;
        lossMentalityOnUnresolved = Constant.ELITE_ENEMY_MENTALITY_PENALTY_AMOUNT;
        reward = new CardEnemyResolveReward(CardRarity.COMMON, CardRarity.RARE);
    }
}