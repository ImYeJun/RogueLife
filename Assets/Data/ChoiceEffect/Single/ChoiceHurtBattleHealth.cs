using System;
using UnityEngine;

[Serializable]
public class ChoiceHurtBattleHealth : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    [SerializeField] private bool isOverflowable = true;

    public ChoiceHurtBattleHealth() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.HurtBattleHealth(amount, isOverflowable);
    }
}