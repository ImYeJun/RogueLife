using System;
using UnityEngine;

[Serializable]
public class ChoiceHurtBattleHealth : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private bool isOverflowable = true;

    public ChoiceHurtBattleHealth() {}

    public void Execute(FieldContext context)
    {
        context.Health.HurtBattleHealth(amount, isOverflowable);
    }
}