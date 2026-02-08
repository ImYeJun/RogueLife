using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxActionCostEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private FieldEffectDuration duration = FieldEffectDuration.ETERNAL;

    public ChoiceIncreaseMaxActionCostEffect() {}

    public void Execute(FieldContext context)
    {
        context.ActionCost.IncreaseMaxCapacity(amount, duration);
    }
}