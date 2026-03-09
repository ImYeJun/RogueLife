using System;
using System.Collections.Generic;

public class BattleScheduler : IBattleScheduler
{
    private BattleContext context;
    private Action<BattleResult> OnBattleEnd;

    public BattleScheduler(Action<BattleResult> onBattleEnd)
    {
        OnBattleEnd = onBattleEnd;
    }

    public void SetContext(BattleContext context) { this.context = context; }
    
    public void StartBattle(BattleStartData data)
    {
        context.EventBus.Publish(new BattleStartEvent(
            data.StartPhaseCount, 
            data.MaxActionCost, 
            data.FirstTurnDrawCount, 
            data.TurnStartDrawCount, 
            data.StartDrawDeck, 
            data.BattlePlayer, 
            data.Enemies
        ));
    }

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

        //TODO : Result에 따른 전투 상황 내의 연출 구현

        OnBattleEnd?.Invoke(result);
    }
}