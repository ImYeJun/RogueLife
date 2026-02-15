using System;
using UnityEngine;

[Serializable]
public class ChoiceRemoveSpecificCardEffect : IChoiceEffect
{
    [SerializeField] private CardData removingCardData;
    [SerializeField, Min(0)] private int amount;

    public ChoiceRemoveSpecificCardEffect() {}

    public void Execute(FieldContext context)
    {
        context.Deck.TryRemoveCardByData(removingCardData, amount);
    }
}