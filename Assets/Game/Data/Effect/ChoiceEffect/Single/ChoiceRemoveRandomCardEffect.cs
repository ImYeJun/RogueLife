using System;
using UnityEngine;

[Serializable]
public class ChoiceRemoveRandomCardEffect : IChoiceEffect
{
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;

    public ChoiceRemoveRandomCardEffect() {}

    public void Execute(FieldContext context)
    {
        context.Deck.TryRemoveRandomCard(type, attribute);
    }
}