using System.Collections.Generic;

public interface IBattleEnemySystemContext {
    public bool IsAnihilated { get; }
    public void SpawnEnemy(BattleEnemy enemy);
    public List<BattleEnemy> GetBattleEnemies();
    public List<BattleEnemy> GetBattleEnemies(EnemyData data);
    public int GetEnemyCountByData(EnemyData data);
}
