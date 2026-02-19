using Battle.HurtSources;

namespace Battle.Cards.Casters
{
    public class EntityCardCaster : CardCaster
    {
        private BattleEntity caster;

        public EntityCardCaster(BattleEntity caster)
        {
            this.caster = caster;
        }

        public override BattleHurtSource GetAsHurtSource()
        {
            return new EntitySource(caster);
        }
    }
}