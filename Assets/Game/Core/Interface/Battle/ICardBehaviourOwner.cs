using Battle.Cards.Casters;
using Battle.HurtSources;

public interface ICardBehaviourOwner
{
    public BattleHurtSource GetAsHurtSource(CardCaster cardCaster);
}