using System;
using UnityEngine;

public class BattleActionCost : IBattleActionCost, IBattleEventObserveService
{
    private int currentActionCost;
    private int maxActionCost;

    private BattleActionCostHistory history;

    public BattleActionCost()
    {
        history = new BattleActionCostHistory();
    }

    public int ActionCost => currentActionCost;
    public int MaxActionCost { get => maxActionCost; set{
            if (value < 0) { UnityEngine.Debug.LogWarning("max action cost cannot be negative."); }
            maxActionCost = value;
        } }
    public BattleActionCostHistory History { get => history; }

    public bool HasEnough(int amount)
    {
        return currentActionCost >= amount;
    }

    public void Consume(int amount)
    {
        int actualAmount = Mathf.Min(amount, currentActionCost);

        currentActionCost -= actualAmount;
        history.RecordConsume(actualAmount);
    }

    public void Restore(int amount)
    {
        int actualAmount = Math.Min(amount, maxActionCost - currentActionCost);

        currentActionCost += actualAmount;
        history.RecordRestore(actualAmount);
    }

    public void Fullfill()
    {
        int restoreAmount = maxActionCost - currentActionCost;
        Restore(restoreAmount);
    }
    
    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiateCost);
        eventBus.Subscribe<PlayerTurnStartBattleEvent>(FullfillOnTurnState);
    }
    public void InitiateCost(BattleStartEvent payload) { maxActionCost = payload.MaxActionCost; }
    public void FullfillOnTurnState(PlayerTurnStartBattleEvent payload) { Fullfill(); }    
}