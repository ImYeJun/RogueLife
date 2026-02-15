using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceIncreaseMaxMentalityEffect() {}

    public void Execute(FieldContext context)
    {
        context.Health.IncreaseMaxMentality(amount);
    }
}