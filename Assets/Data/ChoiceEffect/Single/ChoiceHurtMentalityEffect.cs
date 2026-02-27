using System;
using UnityEngine;

[Serializable]
public class ChoiceHurtMentalityEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceHurtMentalityEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.HurtMentality(amount);
    }
}