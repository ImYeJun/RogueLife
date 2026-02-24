using UnityEngine;

[CreateAssetMenu(fileName = "EliteEnemyData", menuName = "Scriptable Objects/EnemyData/Elite")]
public class EliteEnemyData : EnemyData {
    private void Reset() {
        tier = EnemyTier.ELITE;
    }
}