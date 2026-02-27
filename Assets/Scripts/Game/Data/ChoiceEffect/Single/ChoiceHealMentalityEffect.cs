using System;
using UnityEngine;

[Serializable]
public class ChoiceHealMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private bool isOverflowable = false;

    public ChoiceHealMentalityEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.HealMentality(amount, isOverflowable);
    }
}