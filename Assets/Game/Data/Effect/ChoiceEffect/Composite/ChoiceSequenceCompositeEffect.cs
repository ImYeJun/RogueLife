using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceSequenceCompositeEffect : IChoiceEffect
{
    [SerializeField] List<IChoiceEffect> choiceEffects;
    
    public ChoiceSequenceCompositeEffect() {}

    public void Execute(FieldContext context)
    {
        foreach (var effect in choiceEffects)
        {
            effect.Execute(context);
        }
    }
}