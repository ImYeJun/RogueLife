using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceSequenceCompositeEffect : IChoiceEffect
{
    [SerializeReference, SubclassSelector] List<IChoiceEffect> choiceEffects;
    
    public ChoiceSequenceCompositeEffect() {}

    public void Execute(FieldContext context)
    {
        foreach (var effect in choiceEffects)
        {
            effect.Execute(context);
        }
    }
}