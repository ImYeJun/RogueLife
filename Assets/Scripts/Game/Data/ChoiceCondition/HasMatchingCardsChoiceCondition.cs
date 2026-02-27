using System;
using UnityEngine;

[Serializable]
public class HasMatchingCardsChoiceCondition : IChoiceCondition
{
    [SerializeField] private CardRarity rarity;
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;
    [SerializeField, Min(1)] private int amount;

    public bool IsFulfilled(FieldContext context)
    {
        return context.Deck.HasMatchingCard(rarity, attribute, type, amount);
    }
}