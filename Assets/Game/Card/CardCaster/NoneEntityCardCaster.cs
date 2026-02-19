using Battle.HurtSources;

namespace Battle.Cards.Casters
{
    public class NoneEntityCaster : CardCaster
    {
        public override BattleHurtSource GetAsHurtSource()
        {
            return new NoneEntitySource();
        }
    }
}