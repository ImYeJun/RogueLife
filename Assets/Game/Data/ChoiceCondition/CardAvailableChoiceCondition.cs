using System;
using UnityEngine;

[Serializable]
public class CardAvailableChoiceCondition : IChoiceCondition
{
    [SerializeField] private CardData cardData;
    [SerializeField] private int amount;

    public bool IsFulfilled(FieldContext context)
    {
        return context.Deck.HasEnoughCard(cardData, amount);
    }
}