using System;
using UnityEngine;

[Serializable]
public class ChoiceRemoveRandomCardEffect : IChoiceEffect
{
    [SerializeField] private CardRarity rarity;
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;

    public ChoiceRemoveRandomCardEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.Deck.TryRemoveRandomCard(context.Random, type, attribute);
    }
}