using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalEnemyData", menuName = "Scriptable Objects/EnemyData/Normal")]
public class NormalEnemyData : EnemyData {
    private void Reset() {
        tier = EnemyTier.NORMAL;
        lossMentalityOnUnresolved = Constant.NORMAL_ENEMY_MENTALITY_PENALTY_AMOUNT;
        reward = new CardEnemyResolveReward(CardRarity.COMMON, CardRarity.COMMON);
    }
}
