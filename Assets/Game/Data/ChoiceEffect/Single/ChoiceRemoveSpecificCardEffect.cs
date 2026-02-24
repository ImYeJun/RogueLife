using System;
using UnityEngine;

[Serializable]
public class ChoiceRemoveSpecificCardEffect : IChoiceEffect
{
    [SerializeField] private CardData removingCardData;
    [SerializeField, Min(0)] private int amount;

    public ChoiceRemoveSpecificCardEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Deck.TryRemoveCardByData(removingCardData, amount);
    }
}