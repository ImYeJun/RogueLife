using System;
using System.Collections.Generic;

[Serializable]
public class AllEnemyCardTarget : CardTarget
{
    private List<BattleEnemy> enemies;

    public AllEnemyCardTarget(List<BattleEnemy> enemies)
    {
        this.enemies = enemies;
    }

    public List<BattleEnemy> Enemies { get => enemies; }
}