public class ApplyEntityDebuffBattleAction : IBattleAction
{
    private BattleEntity target;
    private BattleStatusEffect debuff;

    public ApplyEntityDebuffBattleAction(BattleEntity target, BattleStatusEffect debuff)
    {
        this.target = target;
        this.debuff = debuff;
    }

    public BattleEntity Target { get => target; }
    public BattleStatusEffect Debuff { get => debuff; }

    public void Execute(BattleContext context)
    {
        target.ApplyDebuff(debuff);
    }
}