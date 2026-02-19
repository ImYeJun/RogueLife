public class RequestPlayerTurnEndBattleAction : IBattleAction
{
    public void Execute(BattleContext context)
    {
        context.BattleScheduler.EndPlayerTurn();
    }
}