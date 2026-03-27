using UnityEngine;
using ViewEvent.BattleView;

public class BattlePhase : IBattlePhaseContext, IBattleEventObserveService
{
    private BattleContext context;
    private int remainTurn;
    private IBattleViewEventPublisher viewEventPublisher;

    public BattlePhase(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
    }

    public bool IsAllTurnEnd => remainTurn <= 0;
    public int ReaminTurn => remainTurn;

    public void SetContext(BattleContext context) { this.context = context; }
    
    public void Increase(int amount)
    {
        remainTurn += amount;

        viewEventPublisher.Publish(new PhaseIncreased(viewEventPublisher.GetNextSequenceId(), amount, remainTurn));
    }

    public void Decrease(int amount = 1)
    {
        remainTurn = Mathf.Max(remainTurn - amount, 0);
        viewEventPublisher.Publish(new PhaseDecreased(viewEventPublisher.GetNextSequenceId(), amount, remainTurn));

        if (remainTurn <= 0) { 
            var action = new RequestBattleEndBattleAction(BattleResultType.ALL_PHASE_END);
            context.ActionScheduler.EnqueueFront(action);
        }
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiatePhase);
        eventBus.Subscribe<PlayerTurnPreEndedBattleEvent>(OnPlayerTurnPreEnded);
        eventBus.Subscribe<EnemyTurnPreEndedBattleEvent>(OnEnemyTurnPreEnded);
    }

    public void InitiatePhase(BattleStartEvent payload) { 
        remainTurn = payload.StartPhaseCount;

        viewEventPublisher.Publish(new InitialPhaseSettled(viewEventPublisher.GetNextSequenceId(), this));
    }
    public void OnPlayerTurnPreEnded(PlayerTurnPreEndedBattleEvent payload) { Decrease(); }
    public void OnEnemyTurnPreEnded(EnemyTurnPreEndedBattleEvent payload) { Decrease(); }
}