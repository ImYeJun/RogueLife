using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainBeloingingsEffect : IChoiceEffect
{
    [SerializeField] private BelongingsData obtainingBelongingsData;

    public ChoiceObtainBeloingingsEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {   
        Belongings belongings = context.BelongingsDatabase.Materialize(obtainingBelongingsData);
        context.BelongingsBag.TryObtainBelongings(belongings);
    }
}