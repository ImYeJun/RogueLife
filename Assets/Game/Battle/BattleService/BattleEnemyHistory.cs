using System;
using System.Collections.Generic;

public class BattleEnemyHistory : IBattleEnemyHistoryContext
{
    public bool HasAnyoneHurt(BattleScope scope)
    {
        throw new NotImplementedException();
    }

    public HashSet<BattleEnemy> HurtEnemies(BattleScope scope)
    {
        throw new NotImplementedException();
    }
}