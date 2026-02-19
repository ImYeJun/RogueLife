using Battle.Cards.Casters;

public class UseCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardCaster caster;
    private CardTarget target;

    public UseCardEffectBattleAction(Card card, CardCaster caster, CardTarget target)
    {
        this.card = card;
        this.caster = caster;
        this.target = target;
    }

    public void Execute(BattleContext context)
    {
        card.Execute(context, caster, target);
    }
}