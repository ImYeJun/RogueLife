using System;
using UnityEngine;

[Serializable]
public class ChoiceIncreaseMaxCardVarietyEffect : IChoiceEffect
{
    [SerializeField, Min(0)] private int amount;
    
    public ChoiceIncreaseMaxCardVarietyEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Deck.IncreaseMaxCardVariety(amount);
    }
}