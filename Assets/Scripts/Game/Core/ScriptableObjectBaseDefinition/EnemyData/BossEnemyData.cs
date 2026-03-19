using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Scriptable Objects/EnemyData/Boss")]
public class BossEnemyData : EnemyData {
    [SerializeField] private AudioClip battleBgm;

    private void Reset() {
        tier = EnemyTier.BOSS;
    }
}