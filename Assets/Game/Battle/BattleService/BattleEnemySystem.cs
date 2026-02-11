using System;
using System.Collections.Generic;

public class BattleEnemySystem : IBattleEnemySystemContext, IBattleEventObserver
{
    private Dictionary<EnemyData, HashSet<BattleEnemy>> currentEnemies;

    private BattleEnemyHistory history;

    public void SpawnEnemy(BattleEnemy enemy)
    {
        throw new NotImplementedException();
    }

    public HashSet<BattleEnemy> GetBattleEnemies()
    {
        throw new NotImplementedException();
    }

    public int GetEnemyCountByData(EnemyData data)
    {
        throw new NotImplementedException();
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        throw new NotImplementedException();
    }
}