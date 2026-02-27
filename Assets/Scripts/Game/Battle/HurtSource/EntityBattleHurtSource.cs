namespace Battle.HurtSources
{
    public class EntitySource : BattleHurtSource
    {
        protected BattleEntity sourceEntity;

        public EntitySource(BattleEntity sourceEntity) : base(sourceEntity)
        {
            this.sourceEntity = sourceEntity;
        }

        public BattleEntity SourceEntity { get => sourceEntity; }
    }
}