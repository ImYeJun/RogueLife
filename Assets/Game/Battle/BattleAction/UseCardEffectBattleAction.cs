using Battle.Cards.Casters;

public class UseCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardCaster caster;
    private CardTarget target;
    private int executeTimes;

    public UseCardEffectBattleAction(Card card, CardCaster caster, CardTarget target, int executeTimes)
    {
        this.card = card;
        this.caster = caster;
        this.target = target;
        this.executeTimes = executeTimes;
    }

    public void Execute(BattleContext context)
    {
        for (int i = 0; i < executeTimes; i++)
        {
            card.Execute(context, caster, target);
        }
    }
}