using Battle.HurtSources;

namespace Battle.Cards.Casters
{
    public abstract class CardCaster
    {
        public abstract BattleHurtSource GetAsHurtSource();
    }
}