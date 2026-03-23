public class RequestBattleEndBattleAction : IBattleAction
{
    private BattleResultType result;

    public RequestBattleEndBattleAction(BattleResultType result)
    {
        this.result = result;
    }

    public void Execute(BattleContext context)
    {
        switch (result)
        {
            case BattleResultType.PLAYER_ANNIHILATE_WIN:
                if (!context.EnemySystem.IsAnihilated) { return; }
                break;
            case BattleResultType.PLAYER_DIED:
                if (!context.PlayerContainer.Player.IsDead) { return; }
                break;
            case BattleResultType.ALL_PHASE_END:
                if (!context.Phase.IsAllTurnEnd) { return; }
                break;
        }

        context.BattleScheduler.EndBattle(result);
    }
}