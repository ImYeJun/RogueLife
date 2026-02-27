public class KillEntityBattleAction : IBattleAction, IEntityTargetedBattleAction
{
    private BattleEntity target;

    public KillEntityBattleAction(BattleEntity target)
    {
        this.target = target;
    }

    public BattleEntity Target => target;

    public void Execute(BattleContext context)
    {
        target.Kill();
    }
}