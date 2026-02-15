using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainBeloingingsEffect : IChoiceEffect
{
    [SerializeField] private BelongingsData obtainingBelongingsData;

    public ChoiceObtainBeloingingsEffect() {}

    public void Execute(FieldContext context)
    {   
        Belongings belongings = context.BelongingsDatabase.Materialize(obtainingBelongingsData);
        context.BelongingsBag.TryObtainBelongings(belongings);
    }
}