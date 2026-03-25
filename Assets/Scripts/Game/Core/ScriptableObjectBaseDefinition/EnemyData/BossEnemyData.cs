using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyData", menuName = "Scriptable Objects/EnemyData/Boss")]
public class BossEnemyData : EnemyData {
    [SerializeField] private AudioData battleBgm;

    public AudioData BattleBgm { get => battleBgm; }

    private void Reset() {
        tier = EnemyTier.BOSS;
    }
}