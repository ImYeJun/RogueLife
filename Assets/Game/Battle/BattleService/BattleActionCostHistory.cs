using System;
using System.Collections.Generic;
using System.Linq;

public class BattleActionCostHistory : IBattleActionCostHistoryContext, IBattleEventObserver
{
    private int phaseIndex;
    private Dictionary<int, int> consumeHistory = new Dictionary<int, int>();
    //* <PhaseIndex, cosnsumeAmountDuringPlayerTurn>
    private Dictionary<int, int> restoreHistory = new Dictionary<int, int>();
    //* PhaseIndex, restoreAmountDuringPlayerTurn>

    public void RecordConsume(int amount)
    {
        if (amount < 0) { UnityEngine.Debug.LogWarning("Consume amount cannot be negative."); }

        consumeHistory[phaseIndex] += amount;
    }
    public void RecordRestore(int amount)
    {
        if (amount < 0) { UnityEngine.Debug.LogWarning("Restore amount cannot be negative."); }
    
        restoreHistory[phaseIndex] += amount;
    }

    public int GetConsumedActionCostCount(BattleScope scope)
    {
        switch (scope)
        {
            case BattleScope.PHASE:
                return consumeHistory[phaseIndex];
            case BattleScope.BATTLE:
                return consumeHistory.Values.Sum();
            default:
                throw new InvalidOperationException($"{scope} is not valid for searching action cost history.");
        }
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is BattleStartEvent)
        {
            consumeHistory.Clear();
            restoreHistory.Clear();
            phaseIndex = -1;
        }

        if (battleEvent is PhaseStartBattleEvent)
        {
            phaseIndex++;
            consumeHistory[phaseIndex] = 0;
            restoreHistory[phaseIndex] = 0;
        }
    }

}