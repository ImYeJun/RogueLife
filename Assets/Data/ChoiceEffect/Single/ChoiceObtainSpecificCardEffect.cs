using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainSpecificCardEffect : IChoiceEffect
{
    [SerializeField] private CardData obtainingCardData;

    public ChoiceObtainSpecificCardEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        Card card = context.CardDatabase.Materialize(obtainingCardData.Id);
        context.Deck.TryObtainCard(card);
    }
}