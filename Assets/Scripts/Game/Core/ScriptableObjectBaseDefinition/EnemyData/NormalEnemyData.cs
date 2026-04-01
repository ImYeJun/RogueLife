using UnityEngine;

[CreateAssetMenu(fileName = "NormalEnemyData", menuName = "Scriptable Objects/EnemyData/Normal")]
public class NormalEnemyData : EnemyData {
    private void Reset() {
        tier = EnemyTier.NORMAL;
    }
}
