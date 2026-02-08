using System;
using UnityEngine;

[Serializable]
public class ChoiceDecreaseMaxCardVarietyEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;

    public ChoiceDecreaseMaxCardVarietyEffect() {}

    public void Execute(FieldContext context)
    {
        context.Deck.DecreaseMaxCardVariety(amount);
    }
}