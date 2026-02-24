using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainRandomCardEffect : IChoiceEffect
{
    [SerializeField] private CardRarity rarity;
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;

    public ChoiceObtainRandomCardEffect(){}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        Card card = context.CardDatabase.GetRandomCard(context.Random, rarity, type, attribute);
        context.Deck.TryObtainCard(card);
    }
}