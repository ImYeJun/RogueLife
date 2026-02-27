using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ChoiceSequenceCompositeEffect : IChoiceEffect
{
    [SerializeReference, SubclassSelector] List<IChoiceEffect> choiceEffects;
    
    public ChoiceSequenceCompositeEffect() {}

    public bool IsInstant => choiceEffects.All(effect => effect.IsInstant);

    public void Execute(FieldContext context, Node currentNode)
    {
        foreach (var effect in choiceEffects)
        {
            effect.Execute(context, currentNode);
        }
    }
}