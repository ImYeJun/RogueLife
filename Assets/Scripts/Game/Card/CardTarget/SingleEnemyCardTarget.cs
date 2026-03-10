using System.Collections.Generic;

public class SingleEnemyCardTarget : CardTarget
{
    private BattleEnemy enemy;

    public SingleEnemyCardTarget(IReadOnlyBattleEnemy enemy)
    {
        this.enemy = (BattleEnemy)enemy;
        //TODO Hard Refactor is needed to remove the hard Casting
    }

    public BattleEnemy Enemy { get => enemy; }
}