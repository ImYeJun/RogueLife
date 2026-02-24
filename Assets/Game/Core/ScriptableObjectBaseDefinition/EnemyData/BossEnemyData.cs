using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Scriptable Objects/EnemyData/Boss")]
public class BossEnemyData : EnemyData {
    private void Reset() {
        tier = EnemyTier.BOSS;
    }
}