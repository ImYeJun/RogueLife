using System;
using UnityEngine;

[Serializable]
public class ChoiceDecreaseMaxCardVarietyEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceDecreaseMaxCardVarietyEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Deck.DecreaseMaxCardVariety(amount);
    }
}