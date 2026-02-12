using System.Collections.Generic;

public class EnemyCardTarget : CardTarget
{
    private List<BattleEnemy> enemies;

    public EnemyCardTarget(List<BattleEnemy> enemies)
    {
        this.enemies = enemies;
    }

    public List<BattleEnemy> Enemies { get => enemies; }
}