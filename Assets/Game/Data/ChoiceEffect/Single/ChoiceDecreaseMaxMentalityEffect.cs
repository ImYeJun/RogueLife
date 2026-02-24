using System;
using UnityEngine;

[Serializable]
public class ChoiceDecreaseMaxMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceDecreaseMaxMentalityEffect() {}

    public bool IsInstant => true;
    
    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.DecreaseMaxMentality(amount);
    }
}