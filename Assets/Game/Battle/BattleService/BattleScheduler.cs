using System;

public class BattleScheduler : IBattleScheduler
{
    private BattleContext context;
    private Action<BattleResult> onBattleEnd;

    public void StartPhase()
    {
        context.EventBus.Publish(new PhaseStartBattleEvent());
    }

    public void StartPlayerTurn()
    {
        context.EventBus.Publish(new PlayerTurnStartBattleEvent());
    }

    public void EndPlayerTurn()
    {
        context.EventBus.Publish(new PlayerTurnEndBattleEvent());
    }

    public void StartEnemyTurn()
    {
        context.EventBus.Publish(new EnemyTurnStartBattleEvent());
    }

    public void EndEnemyTurn()
    {
        context.EventBus.Publish(new EnemyTurnEndBattleEvent());
    }

    public void EndPhase()
    {
        context.EventBus.Publish(new PhaseEndBattleEvent());
    }

    public void EndBattle(BattleResult result)
    {
        context.EventBus.Publish(new BattleEndBattleEvent(result));

        onBattleEnd?.Invoke(result);
    }
}