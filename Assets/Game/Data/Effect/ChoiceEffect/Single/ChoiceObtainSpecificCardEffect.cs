using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainSpecificCardEffect : IChoiceEffect
{
    [SerializeField] private CardData obtainingCardData;

    public ChoiceObtainSpecificCardEffect() {}

    public void Execute(FieldContext context)
    {
        Card card = context.CardDatabase.MaterializeCardData(obtainingCardData);
        context.Deck.TryObtainCard(card);
    }
}