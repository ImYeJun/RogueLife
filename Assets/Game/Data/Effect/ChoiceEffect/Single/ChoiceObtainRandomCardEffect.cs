using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainRandomCardEffect : IChoiceEffect
{
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;

    public ChoiceObtainRandomCardEffect(){}

    public void Execute(FieldContext context)
    {
        Card card = context.CardDatabase.GetRandomCard(type, attribute);
        context.Deck.TryObtainCard(card);
    }
}