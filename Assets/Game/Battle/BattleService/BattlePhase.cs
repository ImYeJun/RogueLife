using UnityEngine;

public class BattlePhase : IBattlePhaseContext, IBattleEventObserveService
{
    private BattleContext context;
    private int remainPhase;

    public void SetContext(BattleContext context) { this.context = context; }

    public void Increase(int amount)
    {
        remainPhase += amount;
    }

    public void Decrease(int amount = 1)
    {
        remainPhase = Mathf.Max(remainPhase - amount, 0);

        if (remainPhase <= 0) { context.BattleScheduler.EndBattle(BattleResult.ALL_PHASE_END); }
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiatePhase);
        eventBus.Subscribe<PhaseEndBattleEvent>(OnPhaseEnd);
    }

    public void InitiatePhase(BattleStartEvent payload) { remainPhase = payload.StartPhaseCount; }
    public void OnPhaseEnd(PhaseEndBattleEvent payload) { Decrease(); }
}