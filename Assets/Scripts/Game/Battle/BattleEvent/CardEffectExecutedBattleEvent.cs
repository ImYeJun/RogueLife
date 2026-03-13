using Battle.Cards.Casters;

public class CardEffectExecutedBattleEvent : BattleEvent{
    private readonly IReadOnlyBattleCard executedCard;
    private readonly CardCaster caster;
    private readonly CardTarget target;

    public CardEffectExecutedBattleEvent(IReadOnlyBattleCard executedCard, CardCaster caster, CardTarget target)
    {
        this.executedCard = executedCard;
        this.caster = caster;
        this.target = target;
    }

    public IReadOnlyBattleCard ExecutedCard => executedCard;
    public CardCaster Caster => caster;
    public CardTarget Target => target;
}