public abstract class EntityBattleHurtSource : BattleHurtSource
{
    protected BattleEntity sourceEntity;

    protected EntityBattleHurtSource(BattleEntity sourceEntity)
    {
        this.sourceEntity = sourceEntity;
    }

    public BattleEntity SourceEntity { get => sourceEntity; }
}