using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceIncreaseMaxMentalityEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.IncreaseMaxMentality(amount);
    }
}