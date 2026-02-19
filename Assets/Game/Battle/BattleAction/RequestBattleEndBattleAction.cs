public class RequestBattleEndBattleAction : IBattleAction
{
    private BattleResult result;

    public RequestBattleEndBattleAction(BattleResult result)
    {
        this.result = result;
    }

    public void Execute(BattleContext context)
    {
        context.BattleScheduler.EndBattle(result);
    }
}