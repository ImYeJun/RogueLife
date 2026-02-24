using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxBattleHealthEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceIncreaseMaxBattleHealthEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.IncreaseMaxBattleHealth(amount);
    }
}