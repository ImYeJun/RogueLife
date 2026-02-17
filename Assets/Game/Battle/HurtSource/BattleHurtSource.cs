#nullable enable

namespace Battle.HurtSource
{
    public abstract class BattleHurtSource
    {
        private BattleEntity? caster;

        protected BattleHurtSource(BattleEntity? caster = null)
        {
            this.caster = caster;
        }

        public BattleEntity? Caster => caster;
    }
}