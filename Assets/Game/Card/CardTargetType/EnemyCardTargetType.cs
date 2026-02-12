public class EnemyCardTargetType : CardTargetType
{
    private int targetCount;

    public EnemyCardTargetType(int targetCount)
    {
        this.targetCount = targetCount;
    }

    public int TargetCount { get => targetCount; }

    public override bool IsValid(CardTarget target, BattleContext context)
    {
        if (target is EnemyCardTarget enemyCardTarget)
        {
            return targetCount == enemyCardTarget.Enemies.Count;                                                                    
        }

        return false;
    }
}