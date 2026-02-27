public class SpawnEnemyBattleAction : IBattleAction
{
    private BattleEnemy enemy;

    public SpawnEnemyBattleAction(BattleEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute(BattleContext context)
    {
        context.EnemySystem.SpawnEnemy(enemy);
    }
}