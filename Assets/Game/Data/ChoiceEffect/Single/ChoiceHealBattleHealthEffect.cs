using System;
using UnityEngine;

[Serializable]
public class ChoiceHealBattleHealthEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceHealBattleHealthEffect() {}

    public void Execute(FieldContext context)
    {
        context.Health.HealBattleHealth(amount);
    }
}