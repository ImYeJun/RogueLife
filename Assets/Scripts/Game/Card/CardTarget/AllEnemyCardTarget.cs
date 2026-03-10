using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class AllEnemyCardTarget : CardTarget
{
    private List<BattleEnemy> enemies;

    public AllEnemyCardTarget(List<IReadOnlyBattleEnemy> enemies)
    {
        this.enemies = enemies.Select(readOnly => (BattleEnemy)readOnly).ToList();
        //TODO Hard Refactor is needed to remove the hard Casting
    }

    public List<BattleEnemy> Enemies { get => enemies; }
}