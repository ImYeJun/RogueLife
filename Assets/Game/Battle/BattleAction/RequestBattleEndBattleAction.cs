public class RequestBattleEndBattleAction : IBattleAction
{
    private BattleResult result;

    public RequestBattleEndBattleAction(BattleResult result)
    {
        this.result = result;
    }

    public void Execute(BattleContext context)
    {
        switch (result)
        {
            case BattleResult.PLAYER_ANNIHILATE_WIN:
                if (!context.EnemySystem.IsAnihilated) { return; }
                break;
            case BattleResult.PLAYER_DIED:
                if (!context.PlayerContainer.Player.IsDead) { return; }
                break;
            case BattleResult.ALL_PHASE_END:
                if (!context.Phase.IsAllPhasedEnd) { return; }
                break;
        }

        context.BattleScheduler.EndBattle(result);
    }
}