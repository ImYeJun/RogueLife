using System.Collections.Generic;

public class TargetBattleEntity
{
    private BattlePlayer player;
    private List<BattleEnemy> targetedEnemies;

    public TargetBattleEntity(BattlePlayer player, List<BattleEnemy> targetedEnemies)
    {
        this.player = player;
        this.targetedEnemies = targetedEnemies;
    }

    public BattlePlayer Player { get => player; }
    public List<BattleEnemy> TargetedEnemies { get => targetedEnemies; }
}