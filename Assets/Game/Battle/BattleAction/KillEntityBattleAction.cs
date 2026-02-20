public class KillEntityBattleAction : IBattleAction
{
    private BattleEntity target;

    public KillEntityBattleAction(BattleEntity target)
    {
        this.target = target;
    }

    public void Execute(BattleContext context)
    {
        target.Kill();
    }
}