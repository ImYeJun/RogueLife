using System;
using UnityEngine;

[Serializable]
public class ChoiceDecreaseMaxBattleHealthEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceDecreaseMaxBattleHealthEffect() {}

    public void Execute(FieldContext context)
    {
        context.Health.DecreaseMaxBattleHealth(amount);
    }
}