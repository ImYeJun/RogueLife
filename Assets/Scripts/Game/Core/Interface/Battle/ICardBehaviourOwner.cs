using Battle.Cards.Casters;
using Battle.HurtSources;

public interface ICardBehaviourOwner
{
    public BattleHurtSource GetAsHurtSource(CardCaster cardCaster);
    public int CurrentActionCost { get; }
    public CardData GetAsData { get; }

    public void AddCostModifier(CardCostModifier modifier);
    public void RemoveCostModifier(CardCostModifier modifier);
}