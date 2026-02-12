public class EnemyHurtSource : HurtSource
{
    private BattleEnemy sourceEntity;

    public EnemyHurtSource(BattleEnemy sourceEntity)
    {
        this.sourceEntity = sourceEntity;
    }

    public BattleEnemy SourceEntity { get => sourceEntity; }
}