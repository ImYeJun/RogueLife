using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainBeloingingsEffect : IChoiceEffect
{
    [SerializeField] private BelongingsEntity obtainingBelongingsEntity;

    public ChoiceObtainBeloingingsEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {   
        Belongings belongings = context.BelongingsDatabase.Materialize(obtainingBelongingsEntity);
        context.BelongingsBag.TryObtainBelongings(belongings);
    }
}