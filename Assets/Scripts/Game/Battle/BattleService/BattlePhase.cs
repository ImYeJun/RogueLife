using UnityEngine;
using ViewEvent.BattleView;

public class BattlePhase : IBattlePhaseContext, IBattleEventObserveService
{
    private BattleContext context;
    private int remainPhase;
    private IBattleViewEventPublisher viewEventPublisher;

    public BattlePhase(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
    }

    public bool IsAllPhasedEnd => remainPhase <= 0;
    public int ReaminPhase => remainPhase;

    public void SetContext(BattleContext context) { this.context = context; }
    
    public void Increase(int amount)
    {
        remainPhase += amount;
    }

    public void Decrease(int amount = 1)
    {
        remainPhase = Mathf.Max(remainPhase - amount, 0);

        if (remainPhase <= 0) { 
            var action = new RequestBattleEndBattleAction(BattleResult.ALL_PHASE_END);
            context.ActionScheduler.EnqueueFront(action);
        }
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiatePhase);
        eventBus.Subscribe<PhaseEndBattleEvent>(OnPhaseEnd);
    }

    public void InitiatePhase(BattleStartEvent payload) { 
        remainPhase = payload.StartPhaseCount;

        viewEventPublisher.Publish(new InitialPhaseSettled(viewEventPublisher.GetNextSequenceId(), this));
    }
    public void OnPhaseEnd(PhaseEndBattleEvent payload) { Decrease(); }
}