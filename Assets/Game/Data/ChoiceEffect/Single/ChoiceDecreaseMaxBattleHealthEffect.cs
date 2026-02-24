using System;
using UnityEngine;

[Serializable]
public class ChoiceDecreaseMaxBattleHealthEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceDecreaseMaxBattleHealthEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Health.DecreaseMaxBattleHealth(amount);
    }
}