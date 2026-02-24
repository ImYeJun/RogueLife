using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxActionCostEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private FieldEffectDuration duration = FieldEffectDuration.ETERNAL;

    public ChoiceIncreaseMaxActionCostEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.ActionCost.IncreaseMaxCapacity(amount, duration);
    }
}