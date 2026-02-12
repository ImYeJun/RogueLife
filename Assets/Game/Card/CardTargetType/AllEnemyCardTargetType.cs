public class AllEnemyCardTargetType : CardTargetType
{
    public override bool IsValid(CardTarget target, BattleContext context)
    {
        if (target is EnemyCardTarget enemyCardTarget)
        {
            return context.EnemySystem.GetBattleEnemies().Count == enemyCardTarget.Enemies.Count;
        }

        return false;
    }
}