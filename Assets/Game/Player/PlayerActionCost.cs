using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActionCost : IFieldActionCost
{
    private int baseMaxActionCost = Constant.BASE_MAX_ACTION_COST;
    public int CurrentMaxActionCost { 
        get => baseMaxActionCost 
                + temporalCostIncreaseModifiers.Sum(modifier => modifier.ModificatedAmount)
                - temporalCostDecreaseModifiers.Sum(modifier => modifier.ModificatedAmount); }

    private List<TemporalActionCostIncreaseModifier> temporalCostIncreaseModifiers = new List<TemporalActionCostIncreaseModifier>();
    private List<TemporalActionCostDecreaseModifier> temporalCostDecreaseModifiers = new List<TemporalActionCostDecreaseModifier>();

    public void IncreaseMaxCapacity(int amount, FieldEffectDuration duration) 
    {
        if (amount < 0) return;

        switch (duration)
        {
            case FieldEffectDuration.SINGLE_BATTLE:
                temporalCostIncreaseModifiers.Add(new TemporalActionCostIncreaseModifier(1, amount)); 
                break;
            case FieldEffectDuration.ETERNAL:
                baseMaxActionCost += amount;
                break;
            default:
                throw new InvalidOperationException($"[PlayerActionCost] {duration} is not valid.");
        }
    }

    public void DecreaseMaxCapacity(int amount, FieldEffectDuration duration) 
    {
        if (amount < 0) return;

        switch (duration)
        {
            case FieldEffectDuration.SINGLE_BATTLE:
                temporalCostDecreaseModifiers.Add(new TemporalActionCostDecreaseModifier(1, amount)); 
                break;
            case FieldEffectDuration.ETERNAL:
                baseMaxActionCost = Mathf.Max(baseMaxActionCost - amount, 0);
                break;
            default:
                throw new InvalidOperationException($"[PlayerActionCost] {duration} is not valid.");
        }
    }

    public void OnBattleEnd()
    {
        for (int i = temporalCostIncreaseModifiers.Count - 1; i >= 0; i--)
        {
            var element = temporalCostIncreaseModifiers[i];
            if (--element.RemainBattleCount == 0)
            {
                temporalCostIncreaseModifiers.RemoveAt(i);
            }
        }

        for (int i = temporalCostDecreaseModifiers.Count - 1; i >= 0; i--)
        {
            var element = temporalCostDecreaseModifiers[i];
            if (--element.RemainBattleCount == 0)
            {
                temporalCostDecreaseModifiers.RemoveAt(i);
            }
        }
    }
}