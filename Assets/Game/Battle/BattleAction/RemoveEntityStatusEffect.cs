public class RemoveEntityStatusEffect : IBattleAction, IEntityTargetedBattleAction
{
    private BattleEntity target;
    private BattleStatusEffect statusEffect;

    public RemoveEntityStatusEffect(BattleEntity target, BattleStatusEffect statusEffect)
    {
        this.target = target;
        this.statusEffect = statusEffect;
    }

    public BattleEntity Target { get => target; }
    public BattleStatusEffect StatusEffect { get => statusEffect; }

    public void Execute(BattleContext context)
    {
        target.RemoveStatusEffect(statusEffect);
    }
}