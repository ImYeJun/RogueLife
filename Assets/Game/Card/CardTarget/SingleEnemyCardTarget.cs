using System.Collections.Generic;

public class SingleEnemyCardTarget : CardTarget
{
    private BattleEnemy enemy;

    public SingleEnemyCardTarget(BattleEnemy enemy)
    {
        this.enemy = enemy;
    }

    public BattleEnemy Enemy { get => enemy; }
}