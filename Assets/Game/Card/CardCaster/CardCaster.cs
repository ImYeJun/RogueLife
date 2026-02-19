#nullable enable

namespace Battle.Cards.Casters
{
    public abstract class CardCaster
    {
        private BattleEntity? caster;

        protected CardCaster(BattleEntity? caster = null)
        {
            this.caster = caster;
        }

        public BattleEntity? Caster { get => caster;  }
    }
}