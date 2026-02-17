public class ApplyEntityStatusEffectBattleAction : IBattleAction
{
    private BattleEntity target;
    private BattleStatusEffect statusEffect;

    public ApplyEntityStatusEffectBattleAction(BattleEntity target, BattleStatusEffect statusEffect)
    {
        this.target = target;
        this.statusEffect = statusEffect;
    }

    public BattleEntity Target { get => target; }
    public BattleStatusEffect StatusEffect { get => statusEffect; }

    public void Execute(BattleContext context)
    {
        target.ApplyStatusEffect(statusEffect);
    }
}