using System;
using UnityEngine;

[Serializable]
public class ChoiceHealMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private bool isOverflowable = false;

    public ChoiceHealMentalityEffect() {}

    public void Execute(FieldContext context)
    {
        context.Health.HealMentality(amount, isOverflowable);
    }
}