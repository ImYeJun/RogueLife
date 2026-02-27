using System;
using UnityEngine;

[Serializable]
public class ChoiceHealBattleHealthEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceHealBattleHealthEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.HealBattleHealth(amount);
    }
}