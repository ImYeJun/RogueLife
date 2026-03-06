using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainRandomBelongings : IChoiceEffect
{
    [SerializeField] private bool ignoringEquippingBelongings = true;
    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        var newBelongings = context.BelongingsDatabase.GetRandomBelongings(context.Random, 
                                                                            ignoringEquippingBelongings ? context.BelongingsBag.EquippingBelongings : null
                                                                            );
        
        if (newBelongings is null) { return; }
        context.BelongingsBag.TryObtainBelongings(newBelongings);
    }
}