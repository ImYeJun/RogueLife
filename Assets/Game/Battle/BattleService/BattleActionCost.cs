using System;
using UnityEngine;

public class BattleActionCost : IBattleActionCost, IBattleEventObserver
{
    private int currentActionCost;
    private int maxActionCost;

    private BattleActionCostHistory history;

    public int ActionCost => currentActionCost;

    public int MaxActionCost { get => maxActionCost; set{
            if (value < 0) { UnityEngine.Debug.LogWarning("max action cost cannot be negative."); }
            maxActionCost = value;
        } }

    public bool HasEnough(int amount)
    {
        return currentActionCost >= amount;
    }

    public void Consume(int amount)
    {
        int actualAmount = Mathf.Min(amount, maxActionCost - currentActionCost);

        currentActionCost -= actualAmount;
        history.RecordConsume(actualAmount);
    }

    public void Restore(int amount)
    {
        int actualAmount = Math.Min(amount, maxActionCost - currentActionCost);

        currentActionCost += actualAmount;
        history.RecordRestore(actualAmount);
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is PlayerTurnStartBattleEvent)
        {
            int restoreAmount = maxActionCost - currentActionCost;
            Restore(restoreAmount);
        }
    }
}