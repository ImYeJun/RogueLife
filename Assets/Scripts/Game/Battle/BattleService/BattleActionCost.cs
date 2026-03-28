using System;
using System.Collections.Generic;
using UnityEngine;
using ViewEvent.BattleView;

public class BattleActionCost : IBattleActionCost, IBattleEventObserveService
{
    private int currentActionCost;
    private int baseMaxActionCost;
    
    private IBattleViewEventPublisher viewEventPublisher;
    private BattleActionCostHistory history;

    private List<BattleMaxActionCostModifier> modifiers;

    public BattleActionCost(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
        history = new BattleActionCostHistory();
        modifiers = new List<BattleMaxActionCostModifier>();
    }

    public int RemainCost => currentActionCost;
    
    public int MaxActionCost 
    { 
        get 
        {
            int modifiedMax = baseMaxActionCost;
            foreach (var modifier in modifiers)
            {
                modifiedMax += modifier.Delta;
            }
            return Mathf.Max(modifiedMax, 0); 
        }
        set
        {
            if (value < 0) { UnityEngine.Debug.LogWarning("max action cost cannot be negative."); }
            int newBase = Mathf.Max(value, 0);
            
            if (baseMaxActionCost != newBase)
            {
                baseMaxActionCost = newBase;
                OnMaxCostChanged();
            }
        } 
    }
    
    public BattleActionCostHistory History => history;

    public void AddModifier(BattleMaxActionCostModifier modifier)
    {
        modifiers.Add(modifier);
        OnMaxCostChanged();
    }

    public void RemoveModifier(BattleMaxActionCostModifier modifier)
    {
        if (modifiers.Remove(modifier))
        {
            OnMaxCostChanged();
        }
    }

    private void OnMaxCostChanged()
    {
        if (currentActionCost > MaxActionCost)
        {
            currentActionCost = MaxActionCost;
        }

        viewEventPublisher.Publish(new MaxCostChanged(viewEventPublisher.GetNextSequenceId(), MaxActionCost, currentActionCost));
    }

    public bool HasEnough(int amount)
    {
        return currentActionCost >= amount;
    }

    public void Consume(int amount)
    {
        int actualAmount = Mathf.Min(amount, currentActionCost);

        currentActionCost -= actualAmount;
        history.RecordConsume(actualAmount);

        viewEventPublisher.Publish(new CostConsumed(viewEventPublisher.GetNextSequenceId(), amount, currentActionCost));
    }

    public void Restore(int amount)
    {
        int actualAmount = Math.Min(amount, MaxActionCost - currentActionCost);

        currentActionCost += actualAmount;
        history.RecordRestore(actualAmount);

        viewEventPublisher.Publish(new CostRestored(viewEventPublisher.GetNextSequenceId(), amount, currentActionCost));
    }

    public void Fullfill()
    {
        int restoreAmount = MaxActionCost - currentActionCost;
        if (restoreAmount > 0)
        {
            Restore(restoreAmount);
        }
    }
    
    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiateCost);
        eventBus.Subscribe<PlayerTurnStartBattleEvent>(FullfillOnTurnState);
        
        eventBus.Subscribe<PlayerTurnEndBattleEvent>(CleanUpTurnModifiers);
        eventBus.Subscribe<PhaseEndBattleEvent>(CleanUpPhaseModifiers);
        eventBus.Subscribe<BattleEndBattleEvent>(CleanUpAllModifiers);
    }

    public void InitiateCost(BattleStartEvent payload) 
    { 
        baseMaxActionCost = payload.MaxActionCost;
        modifiers.Clear();

        viewEventPublisher.Publish(new InitialActionCostSettled(viewEventPublisher.GetNextSequenceId(), this));
    }

    public void FullfillOnTurnState(PlayerTurnStartBattleEvent payload) { Fullfill(); }    

    private void CleanUpTurnModifiers(PlayerTurnEndBattleEvent payload)
    {
        int removedCount = modifiers.RemoveAll(m => m.Scope == BattleScope.TURN);
        if (removedCount > 0) { OnMaxCostChanged(); }
    }

    private void CleanUpPhaseModifiers(PhaseEndBattleEvent payload)
    {
        int removedCount = modifiers.RemoveAll(m => m.Scope == BattleScope.PHASE);
        if (removedCount > 0) { OnMaxCostChanged(); }
    }

    private void CleanUpAllModifiers(BattleEndBattleEvent payload)
    {
        if (modifiers.Count > 0)
        {
            modifiers.Clear();
            OnMaxCostChanged();
        }
    }
}