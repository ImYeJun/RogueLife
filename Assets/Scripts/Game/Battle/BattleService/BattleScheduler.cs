using System;
using System.Collections.Generic;
using ViewEvent.BattleView;

public class BattleScheduler : IBattleScheduler
{
    private bool isBattleActive = false;

    private BattleContext context;
    private IBattleViewEventPublisher viewEventPublisher;
    private Action<BattleResultType> OnBattleEnd;

    public BattleScheduler(Action<BattleResultType> onBattleEnd, IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
        OnBattleEnd = onBattleEnd;
    }

    public void SetContext(BattleContext context) { this.context = context; }
    
    public void StartBattle(BattleStartData data)
    {
        isBattleActive = true;

        context.EventBus.Publish(new BattleStartEvent(
            data.StartPhaseCount, 
            data.MaxActionCost, 
            data.FirstTurnDrawCount, 
            data.TurnStartDrawCount, 
            data.StartDrawDeck, 
            data.BattlePlayer, 
            data.BattleBelongings,
            data.Enemies,
            data.MainEnemyData
        ));
        viewEventPublisher.Publish(new BattleStarted(viewEventPublisher.GetNextSequenceId(), data.MainEnemyData));

        StartPhase();
    }

    public void StartPhase()
    {
        if (!isBattleActive) { return; }

        viewEventPublisher.Publish(new PhaseStarted(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new PhaseStartBattleEvent());

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (!isBattleActive) { return; }

        viewEventPublisher.Publish(new PlayerTurnStarted(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new PlayerTurnStartBattleEvent());
    }

    public void EndPlayerTurn()
    {
        if (!isBattleActive) { return; }

        context.EventBus.Publish(new PlayerTurnPreEndedBattleEvent());
        viewEventPublisher.Publish(new PlayerTurnEnded(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new PlayerTurnEndBattleEvent());

        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        if (!isBattleActive) { return; }

        viewEventPublisher.Publish(new EnemyTurnStarted(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new EnemyTurnStartBattleEvent());
    }

    public void EndEnemyTurn()
    {
        if (!isBattleActive) { return; }

        context.EventBus.Publish(new EnemyTurnPreEndedBattleEvent());
        viewEventPublisher.Publish(new EnemyTurnEnded(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new EnemyTurnEndBattleEvent());

        EndPhase();
    }

    public void EndPhase()
    {
        if (!isBattleActive) { return; }

        viewEventPublisher.Publish(new PhaseEnded(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new PhaseEndBattleEvent());

        StartPhase();
    }

    public void EndBattle(BattleResultType result)
    {
        if (!isBattleActive) { return; }

        viewEventPublisher.Publish(new BattleEnded(viewEventPublisher.GetNextSequenceId()));
        context.EventBus.Publish(new BattleEndBattleEvent(result));
        
        OnBattleEnd?.Invoke(result);
        isBattleActive = false;
    }
}