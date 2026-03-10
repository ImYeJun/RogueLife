using System;
using System.Collections.Generic;
using ViewEvent.BattleView;

public class BattleScheduler : IBattleScheduler
{
    private BattleContext context;
    private IBattleViewEventPublisher viewEventPublisher;
    private Action<BattleResult> OnBattleEnd;

    public BattleScheduler(Action<BattleResult> onBattleEnd, IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
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
            data.BattleBelongings,
            data.Enemies
        ));
        viewEventPublisher.Publish(new BattleStarted(viewEventPublisher.GetNextSequenceId()));

        StartPhase();
    }

    public void StartPhase()
    {
        context.EventBus.Publish(new PhaseStartBattleEvent());
        viewEventPublisher.Publish(new PhaseStarted(viewEventPublisher.GetNextSequenceId()));

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        context.EventBus.Publish(new PlayerTurnStartBattleEvent());
        viewEventPublisher.Publish(new PlayerTurnStarted(viewEventPublisher.GetNextSequenceId()));
    }

    public void EndPlayerTurn()
    {
        context.EventBus.Publish(new PlayerTurnEndBattleEvent());
        viewEventPublisher.Publish(new PlayerTurnEnded(viewEventPublisher.GetNextSequenceId()));
    }

    public void StartEnemyTurn()
    {
        context.EventBus.Publish(new EnemyTurnStartBattleEvent());
        viewEventPublisher.Publish(new EnemyTurnStarted(viewEventPublisher.GetNextSequenceId()));
    }

    public void EndEnemyTurn()
    {
        context.EventBus.Publish(new EnemyTurnEndBattleEvent());
        viewEventPublisher.Publish(new EnemyTurnEnded(viewEventPublisher.GetNextSequenceId()));
    }

    public void EndPhase()
    {
        context.EventBus.Publish(new PhaseEndBattleEvent());
        viewEventPublisher.Publish(new PhaseEnded(viewEventPublisher.GetNextSequenceId()));
    }

    public void EndBattle(BattleResult result)
    {
        context.EventBus.Publish(new BattleEndBattleEvent(result));
        viewEventPublisher.Publish(new BattleEnded(viewEventPublisher.GetNextSequenceId()));
        
        OnBattleEnd?.Invoke(result);
    }
}