public class ApplyEntityBuffBattleAction : IBattleAction
{
    private BattleEntity target;
    private BattleStatusEffect buff;

    public ApplyEntityBuffBattleAction(BattleEntity target, BattleStatusEffect buff)
    {
        this.target = target;
        this.buff = buff;
    }

    public BattleEntity Target { get => target; }
    public BattleStatusEffect Buff { get => buff; }

    public void Execute(BattleContext context)
    {
        target.ApplyBuff(buff);
    }
}