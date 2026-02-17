using System.Collections.Generic;

public interface IBattleEnemySystemContext {
    public void SpawnEnemy(BattleEnemy enemy);
    public List<BattleEnemy> GetBattleEnemies();
    public int GetEnemyCountByData(EnemyData data);
}