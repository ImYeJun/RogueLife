using UnityEngine;

public class BattlePhase : IBattlePhaseContext, IBattleEventObserver
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

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is BattleStartEvent payload)
        {
            remainPhase = payload.StartPhaseCount;
        }

        if (battleEvent is PhaseEndBattleEvent)
        {
            Decrease();
        }
    }
}