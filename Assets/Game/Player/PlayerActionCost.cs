using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCost : IFieldActionCost
{
    private int maxActionCost = Constant.BASE_MAX_ACTION_COST;
    public int MaxActionCost { get => maxActionCost; }

    private List<TemporalActionCostIncreaseModifier> temporalCostIncreaseModifiers = new List<TemporalActionCostIncreaseModifier>();
    private List<TemporalActionCostDecreaseModifier> temporalCostDecreaseModifiers = new List<TemporalActionCostDecreaseModifier>();

    // public bool TrySpend(int amount)
    // {
    //     if (amount < 0) { return false; }
    //     if (currentActionCost < amount) { return false; }

    //     currentActionCost -= amount;
    //     return true;
    // }

    // public void Refill() { currentActionCost = maxActionCost; }

    public void IncreaseMaxCapacity(int amount, FieldEffectDuration duration) 
    {
        if (amount < 0) return;

        maxActionCost += amount;

        if (duration == FieldEffectDuration.SINGLE_BATTLE) 
        { 
            temporalCostIncreaseModifiers.Add(new TemporalActionCostIncreaseModifier(1, amount)); 
        }
    }

    public void DecreaseMaxCapacity(int amount, FieldEffectDuration duration) 
    {
        if (amount < 0) return;

        int actualDecreased = Mathf.Min(maxActionCost, amount);
        
        maxActionCost -= actualDecreased;

        if (duration == FieldEffectDuration.SINGLE_BATTLE && actualDecreased > 0) 
        { 
            temporalCostDecreaseModifiers.Add(new TemporalActionCostDecreaseModifier(1, actualDecreased)); 
        }
    }

    public void OnBattleEnd()
    {
        for (int i = temporalCostIncreaseModifiers.Count - 1; i >= 0; i--)
        {
            var element = temporalCostIncreaseModifiers[i];
            if (--element.RemainBattleCount == 0)
            {
                DecreaseMaxCapacity(element.ModificatedAmount, FieldEffectDuration.ETERNAL);
                temporalCostIncreaseModifiers.RemoveAt(i);
            }
        }

        for (int i = temporalCostDecreaseModifiers.Count - 1; i >= 0; i--)
        {
            var element = temporalCostDecreaseModifiers[i];
            if (--element.RemainBattleCount == 0)
            {
                IncreaseMaxCapacity(element.ModificatedAmount, FieldEffectDuration.ETERNAL);
                temporalCostDecreaseModifiers.RemoveAt(i);
            }
        }
    }
}