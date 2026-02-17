using System.Collections.Generic;

public interface IBattleEnemyHistoryContext {
    public bool HasAnyoneHurt(BattleScope scope);
    public HashSet<BattleEnemy> HurtEnemies(BattleScope scope);

}