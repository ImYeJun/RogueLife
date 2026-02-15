using System;
using UnityEngine;

[Serializable]
public class ChoiceHurtMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceHurtMentalityEffect() {}

    public void Execute(FieldContext context)
    {
        context.Health.HurtMentality(amount);
    }
}