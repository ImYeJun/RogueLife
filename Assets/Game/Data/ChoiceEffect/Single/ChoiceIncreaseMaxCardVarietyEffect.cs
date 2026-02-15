using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxCardVarietyEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    
    public ChoiceIncreaseMaxCardVarietyEffect() {}

    public void Execute(FieldContext context)
    {
        context.Deck.IncreaseMaxCardVariety(amount);
    }
}