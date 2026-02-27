using System;
using UnityEngine;

[Serializable]
public class ChoiceDecreaseMaxActionCostEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private FieldEffectDuration duration = FieldEffectDuration.ETERNAL;

    public ChoiceDecreaseMaxActionCostEffect(){}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.ActionCost.DecreaseMaxCapacity(amount, duration);
    }
}